using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// TODO: replace this with the type you want to read.
using TRead = TowerDefenseXNA.Level;

namespace SharedData
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    public class LevelReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            existingInstance = new TRead();
            existingInstance.playerLife = input.ReadInt32();
            existingInstance.playerMoney = input.ReadInt32();
            existingInstance.tilesPerRow = input.ReadInt32();
            existingInstance.waypoints = input.ReadObject<Queue<Vector2>>();
            existingInstance.tileSize = input.ReadInt32();
            existingInstance.start = input.ReadVector2();
            existingInstance.end = input.ReadVector2();
            existingInstance.map = input.ReadObject<int[][]>();
            existingInstance.wavelist = input.ReadObject<int[][]>();
            return existingInstance;
        }
    }
}
