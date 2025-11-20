using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sayo.Core.Scene;
using System;
using static Sayo.Core.Helper;

namespace Sayo.Core.Object;

/// <summary>
///     游戏中的主角类型,有头身体屁股三种部位,头部负责转向,身体负责显示转向时的不同状态,屁股好像没必要单独分出来一个对象,但是也懒得删了,状态在grid中储存
/// </summary>
internal class SayoPlayer
{
    private readonly SayoHead _head;
    private readonly SayoBody[] _bodys = new SayoBody[400];
    private readonly SayoButt _butt;
    private Grid _grid;
    public static int bodyCount = 1;

    public SayoPlayer(Texture2D[] head, Texture2D[] bodyTexture, Texture2D butt, Grid grid)
    {
        _grid = grid;
        _head = new SayoHead(head[0], head[1]);

        SayoBody.BodyTexture = bodyTexture[0];
        SayoBody.FullBody = bodyTexture[1];
        SayoBody.TurnedBody = bodyTexture[2];
        SayoBody.FullAndTurnedBody = bodyTexture[3];
        SegmentStatus bodyStatus = new()
        {
            TargetPosition = new Point(_head.Status.SourcePosition.X, _head.Status.SourcePosition.Y),
            SourcePosition = new Point(_head.Status.TargetPosition.X - 1, _head.Status.TargetPosition.Y)
        };

        var body = new SayoBody(SayoBody.BodyTexture)
        {
            Status = bodyStatus
        };
        body.Status.TargetPosition = bodyStatus.TargetPosition;

        _bodys[0] = body;

        _bodys[0].Status.SourcePosition.X = 1;
        _butt = new SayoButt(butt)
        {
            Status = new SegmentStatus
            {
                TargetPosition = new Point(_head.Status.TargetPosition.X - 2, _head.Status.TargetPosition.Y),
                SourcePosition = new Point(_head.Status.TargetPosition.X - 3, _head.Status.TargetPosition.Y)
            }
        };
        _butt.OldStatus = _butt.Status;

        grid.Cell[_head.Status.TargetPosition.X, _head.Status.TargetPosition.Y] = _head;
        grid.Cell[_bodys[0].Status.TargetPosition.X, _bodys[0].Status.TargetPosition.Y] = _bodys[0];
        grid.Cell[_butt.Status.TargetPosition.X, _butt.Status.TargetPosition.Y] = _butt;
    }
    private ObjType _isAteSomething;
    public void Update(Keys lastKey, Grid grid, Food food)
    {
        //先更新位置, 统一更新完后移动
        _head.Update(lastKey);

        for (int i = 0; i < bodyCount; i++)
            if (i == 0)
                _bodys[i].Update(_head);
            else
                _bodys[i].Update(_bodys[i - 1]);

        _butt.Update(_bodys[bodyCount - 1]);

        _isAteSomething = IsAteSomething();
        switch (_isAteSomething)
        {
            case ObjType.Air:
                _head.Status.Ate = false;
                _head.CurrectTexture2D = _head.Head;
                try
                {
                    _head.Move(_grid);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                for (int i = 0; i < bodyCount; i++)
                {
                    try
                    {
                        _bodys[i].Move(_grid);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                try
                {
                    _butt.Move(grid);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                break;
            case ObjType.Body:
                GameOver();
                break;
            case ObjType.Food:
                _head.Status.Ate = true;
                food.Set(grid);
                _head.Move(grid);
                _head.CurrectTexture2D = _head.Head_Eating;
                var newBody = SayoBody.AddBody(_bodys, _butt.Status,_bodys[bodyCount-1].OldStatus);
                if (newBody.Status.Turn != Turn.NoTurn)
                {
                    _butt.Status.Rotation = newBody.Status.Turn switch
                    {
                        Turn.上右 => 不旋转,
                        Turn.左下 => 不旋转,
                        Turn.右下 => 旋转90度,
                        Turn.上左 => 旋转90度,
                        Turn.下左 => 旋转180度,
                        Turn.右上 => 旋转180度,
                        Turn.下右 => 旋转270度,
                        Turn.左上 => 旋转270度,
                        _ => 114514
                    };
                }
                for (int i = 0; i < bodyCount - 1; i++)
                {
                    try
                    {
                        _bodys[i].Move(grid);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                if (grid.Cell[newBody.Status.TargetPosition.X, newBody.Status.TargetPosition.Y] == null)
                {
                    grid.Cell[newBody.Status.TargetPosition.X, newBody.Status.TargetPosition.Y] = newBody;
                }
                else
                {
                    throw new Exception("Body位置冲突");
                }
                break;
            case ObjType.Edge:
                GameOver();
                break;
        }
    }

    public ObjType IsAteSomething()
    {
        return _grid.CheckBounds(_head.Status.TargetPosition.X, _head.Status.TargetPosition.Y)
            ? ObjType.Edge
            : _grid.Cell[_head.Status.TargetPosition.X, _head.Status.TargetPosition.Y] == null
            ? ObjType.Air
            : _grid.Cell[_head.Status.TargetPosition.X, _head.Status.TargetPosition.Y].ObjType;
    }
    public static void GameOver()
    {
        GameScene.GameRunning = false;
        SceneManager.ChangeScene("GameOver");
    }
}

internal class SayoHead : Sprite
{
    public Texture2D Head_Eating;
    public Texture2D Head;

    public SayoHead(Texture2D head, Texture2D head_Eating) : base(head)
    {
        ObjType = ObjType.Body;
        Head = head;
        Head_Eating = head_Eating;
    }

    public void Update(Keys lastKey)
    {
        OldStatus = Status;
        Status.Direction = lastKey switch
        {
            Keys.Up when OldStatus.Direction != Direction.Down => Direction.Up,
            Keys.Down when OldStatus.Direction != Direction.Up => Direction.Down,
            Keys.Left when OldStatus.Direction != Direction.Right => Direction.Left,
            Keys.Right when OldStatus.Direction != Direction.Left => Direction.Right,
            _ => OldStatus.Direction
        };
        //根据当前方向和上一个方向判断转向情况
        OldStatus.Turn =
            OldStatus.Direction switch
            {
                Direction.Right when lastKey == Keys.Up => Turn.右上,
                Direction.Down when lastKey == Keys.Left => Turn.下左,

                Direction.Left when lastKey == Keys.Up => Turn.左上,
                Direction.Down when lastKey == Keys.Right => Turn.下右,

                Direction.Left when lastKey == Keys.Down => Turn.左下,
                Direction.Up when lastKey == Keys.Right => Turn.上右,

                Direction.Right when lastKey == Keys.Down => Turn.右下,
                Direction.Up when lastKey == Keys.Left => Turn.上左,
                _ => Turn.NoTurn
            };
        Status.Rotation =
            Status.Direction switch
            {
                Direction.Up => 上方向,
                Direction.Down => 下方向,
                Direction.Left => 左方向,
                Direction.Right => 右方向,
                _ => Status.Rotation
            }
        ;


        var move = Status.Direction switch
        {
            Direction.Up => new Point(0, -1),
            Direction.Down => new Point(0, 1),
            Direction.Left => new Point(-1, 0),
            Direction.Right => new Point(1, 0),
            _ => Point.Zero
        };
        Status.SourcePosition = Status.TargetPosition;
        Status.TargetPosition += move;
    }
}

internal class SayoBody : Sprite
{
    public static Texture2D BodyTexture;
    public static Texture2D TurnedBody;
    public static Texture2D FullBody;
    public static Texture2D FullAndTurnedBody;

    public SayoBody(Texture2D texture2D) : base(texture2D)
    {
        ObjType = ObjType.Body;
    }


    public void Update(Sprite segment)
    {
        OldStatus = Status;
        Status = segment.OldStatus;
        if (Status.Turn != Turn.NoTurn)
        {
            Status.Rotation = Status.Turn switch
            {
                Turn.上右 => 不旋转,
                Turn.左下 => 不旋转,
                Turn.右下 => 旋转90度,
                Turn.上左 => 旋转90度,
                Turn.下左 => 旋转180度,
                Turn.右上 => 旋转180度,
                Turn.下右 => 旋转270度,
                Turn.左上 => 旋转270度,
                _ => Status.Rotation
            };
        }
            bool ate = Status.Ate;
        bool turned = Status.Turn != Turn.NoTurn;

        CurrectTexture2D =
            (ate, turned) switch
            {
                (true, true) => FullAndTurnedBody,
                (true, false) => FullBody,
                (false, true) => TurnedBody,
                (false, false) => BodyTexture
            };
    }

    public static SayoBody AddBody(SayoBody[] bodys, SegmentStatus status,SegmentStatus lastBodyStatus)
    {
        var body = new SayoBody(BodyTexture)
        {
            Status = lastBodyStatus
        };
        body.Status.TargetPosition = status.TargetPosition;

        bodys[SayoPlayer.bodyCount] = body;
        body.OldStatus = status;
        if(body.Status.Turn != Turn.NoTurn)
        {
            body.CurrectTexture2D = TurnedBody;
        }
        SayoPlayer.bodyCount++;
        return body;
    }
}

internal class SayoButt : Sprite
{
    public SayoButt(Texture2D texture2D) : base(texture2D)
    {
        ObjType = ObjType.Body;
    }


    public void Update(SayoBody body)
    {
        OldStatus = Status;
        Status = body.OldStatus;
        if(Status.Turn != Turn.NoTurn)
        {
            Status.Rotation = Status.Turn switch
            {
                Turn.上右 => 旋转90度,
                Turn.下右 => 旋转90度,
                Turn.左下 => 旋转180度,
                Turn.右下 => 旋转180度,
                Turn.上左 => 旋转270度,
                Turn.下左 => 旋转270度,
                Turn.右上 => 不旋转,
                Turn.左上 => 不旋转,
                _ => Status.Rotation
            };
        }
    }
}