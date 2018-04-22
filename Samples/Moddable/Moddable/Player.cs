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
        private const string StepEvent = "step";

        private TsInstance _source;
        private TsDelegate _step = null;
        private Texture2D _texture;

        public float X
        {
            get => (float)_source["x"];
            set => _source["x"] = value;
        }

        public float Y
        {
            get => (float)_source["y"];
            set => _source["y"] = value;
        }

        public Player(string playerType, Texture2D texture)
        {
            if (playerType == Parent || TsInstance.ObjectIsAncestor(playerType, Parent))
            {
                _source = new TsInstance(playerType);
                _step = _source.GetDelegate(StepEvent);
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

        public void Destroy()
        {
            _source.Destroy();
            _source = null;
        }
    }
}
