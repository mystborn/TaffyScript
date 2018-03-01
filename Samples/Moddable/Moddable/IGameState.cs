using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Moddable
{
    public interface IGameState
    {
        void Draw(SpriteBatch batch);
        void Update(GameTime gameTime);
    }
}
