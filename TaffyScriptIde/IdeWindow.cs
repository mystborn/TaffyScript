using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace TaffyScriptIde
{
    public class IdeWindow
    {
        private Window _window;

        public IdeWindow(string windowTitle)
        {
            _window = new Window(windowTitle);
            _window.DeleteEvent += DeleteEvent;
            _window.BorderWidth = 10;
            var button = GenerateButton();
            _window.Add(button);
            _window.ShowAll();
        }

        private Widget GenerateButton()
        {
            var box = new HBox(false, 0);
            box.BorderWidth = 2;
            var image = new Image("Assets/Owl.png");
            var label = new Label("Clickable Owl");
            box.PackStart(image, false, false, 3);
            box.PackStart(label, false, false, 3);

            image.Show();
            label.Show();

            box.Show();

            var button = new Button();
            button.Add(box);
            button.Show();
            return button;
        }

        private void DeleteEvent(object obj, EventArgs args)
        {
            Application.Quit();
        }
    }
}
