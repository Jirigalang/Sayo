using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core;
using Sayo.Core.Object;
using SayoKNI.SayoSegment;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SayoKNI;

public static class SpriteManager
{
    public static List<GameObject> ObjectList { get; set; } = [];
    public static void Draw(SpriteBatch sb)
    {
        sb.Begin();
        foreach (var obj in ObjectList)
        {
            sb.Draw(texture: obj.Sprite.Texture,//图像
                    position: obj.Position,//在画面中绘制的位置
                    sourceRectangle: obj.Sprite.Frames[obj.Sprite.CurrentFrame],//描述单个精灵图在图片中的区域的矩形
                    color: Color.White,//对每个颜色进行像素乘法再输出,白色等于原模原样绘制
                    rotation: obj.Rolation,//旋转角度,弧度制
                    origin: obj.Sprite.Center,//旋转中心
                    scale: 1,//缩放大小,1就是不缩小也不放大
                    effects: SpriteEffects.None,//水平或垂直反转
                    layerDepth: 0);//前后顺序,只有在特定 SpriteSortMode 下才生效
        }
        sb.End();
    }
}