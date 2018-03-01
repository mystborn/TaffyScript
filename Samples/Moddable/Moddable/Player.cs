using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TaffyScript;

namespace Moddable
{
    /// <summary>
    /// Wraps a player TsObject
    /// </summary>
    public class Player
    {
        private const string Parent = "GameBase.obj_player";

        private TsInstance _source;
        private InstanceEvent _step = null;
        private Texture2D _texture;

        public float X
        {
            get => _source["x"].GetNum();
            set => _source["x"] = (TsObject)value;
        }

        public float Y
        {
            get => _source["y"].GetNum();
            set => _source["y"] = (TsObject)value;
        }

        public Player(string playerType, Texture2D texture)
        {
            if (playerType == Parent || TsInstance.ObjectIsAncestor(playerType, Parent))
            {
                _source = TsInstance.InstanceCreate(playerType).GetInstance();
                if (TsInstance.TryGetEvent(playerType, "step", out var step))
                    _step = step;
                _texture = texture;
            }
            else
                throw new InvalidOperationException("Received an invalid player type.");
        }

        public void Step()
        {
            _step?.Invoke(_source);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, new Vector2(X, Y), Color.White);
        }
    }
}
