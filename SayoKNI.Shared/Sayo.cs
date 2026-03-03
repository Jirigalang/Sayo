using Microsoft.Xna.Framework.Input;
using Sayo.Core.Object;
using SayoKNI.SayoSegment;
using System.Collections.Generic;
using System.Linq;
namespace SayoKNI;
internal class Sayo
{
    private readonly Segment[] _sayoBodys = new Segment[20*20];
    private Grid _grid;
    private Food _food;
    private int _bodyCount = 3;
    public int Speed = 1;

    public SayoHead Head => (SayoHead)_sayoBodys[0];
    public SayoBody[] Body => [.. (SayoBody[])_sayoBodys.Skip(1)];

    public Sayo()
    {

    }

    public void Load(SayoHead sayoHead, SayoBody sayoBody, SayoBody sayoButt, Food food, Grid grid)
    {
        _sayoBodys[0] = sayoHead;
        _sayoBodys[1] = sayoBody;
        _sayoBodys[1].Location = new(2, 1);
        _sayoBodys[2] = sayoButt;
        _sayoBodys[2].Location = new(1, 1);
        _food = food;
        _grid = grid;
        _grid.Cell.AddRange([sayoHead, sayoBody, sayoButt]);
    }

    public void Update(Keys lastKey)
    {
        var head = (SayoHead)_sayoBodys[0];
        for (int i = _bodyCount - 1; i >= 1; i--)
        {
            //逆向传递吃没吃的状态
            ((SayoBody)_sayoBodys[i]).UpdateState(_sayoBodys[i - 1]);
        }
        //更新脑袋位置和状态
        head.UpdateLocation(lastKey);
        var isAte = head.SetState(_food);
        if (isAte)
        {
            //增加身体
            var newBody = new SayoBody
            {
                IsButt = true
            };
            var lastButt = (SayoBody)_sayoBodys[_bodyCount - 1];
            lastButt.IsButt = false;
            //复制最后一个身体的位置和方向
            newBody.Location = lastButt.Location;
            newBody.Rolation = lastButt.Rolation;
            _sayoBodys[_bodyCount] = newBody;
            //重新设置食物位置
            _food.Set(_grid);
            _grid.Cell.Add(_sayoBodys[_bodyCount]);
        }
        //更新身体位置和方向
        for (int i = _bodyCount - 1; i >= 1; i--)
        {
            ((SayoBody)_sayoBodys[i]).UpdateLocation(_sayoBodys[i - 1]);
        }
        for (int i = 1; i < _bodyCount; i++)
        {
            var body = ((SayoBody)_sayoBodys[i]);
            body.UpdateDirection(_sayoBodys[i - 1]);
            body.UpdateButt();
        }
        //最后更新身体数量，保证新增身体不更新
        if (isAte)
            _bodyCount++;
        _grid.CheckNotDuplicate();

    }
}