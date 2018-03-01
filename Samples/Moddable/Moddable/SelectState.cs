using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModdableExtern;
using TaffyScript;

namespace Moddable
{
    public class SelectState : IGameState
    {
        private SpriteFont _font;
        private int _selected = 0;
        private List<PlayerSelect> _characterChoices = new List<PlayerSelect>();
        private Game1 _game;

        public SelectState(Game1 game, ContentManager content)
        {
            _font = content.Load<SpriteFont>("font");
            foreach(var type in TsInstance.Types)
            {
                if (TsInstance.ObjectIsAncestor(type, "GameBase.par_character_select"))
                    _characterChoices.Add(new PlayerSelect(type));
            }
            _game = game;
        }

        public void Draw(SpriteBatch batch)
        {
            for(var i = 0; i < _characterChoices.Count; i++)
            {
                var color = i == _selected ? Color.Red : Color.Black;
                batch.DrawString(_font, _characterChoices[i].Name, new Vector2(32, 32 + i * 20), color);
            }
        }

        public void Update(GameTime gameTime)
        {
            if(Input.KeyCheckPressed(Keys.Up))
            {
                if (_selected == 0)
                    _selected = _characterChoices.Count - 1;
                else
                    --_selected;
            }
            else if (Input.KeyCheckPressed(Keys.Down))
            {
                if (_selected == _characterChoices.Count - 1)
                    _selected = 0;
                else
                    ++_selected;
            }
            else if (Input.KeyCheckPressed(Keys.Space))
            {
                var type = _characterChoices[_selected].PlayerObjectType;
                foreach (var choice in _characterChoices)
                    choice.Destroy();
                var play = new PlayState(type, _game.Content);
                _game.ChangeState(play);
            }
        }
    }
}
