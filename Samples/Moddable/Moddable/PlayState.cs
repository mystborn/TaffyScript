using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TaffyScript;

namespace Moddable
{
    public class PlayState : IGameState
    {
        private Player _player;

        public PlayState(string playerType, ContentManager content)
        {
            var texture = content.Load<Texture2D>("Owl_Small");
            _player = new Player(playerType, texture);
        }

        public void Update(GameTime gameTime)
        {
            _player.Step();
        }

        public void Draw(SpriteBatch batch)
        {
            _player.Draw(batch);
        }
    }
}
