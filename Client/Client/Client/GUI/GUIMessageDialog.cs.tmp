using System;
using System.Text;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    public class GUIMessageDialog : Window
    {
        private Label MessageLabel;
        public GUIMessageDialog(Manager manager, String Title, String Message)
            : base(manager)
        {
            Init();
            Text = Title;
            Width = 330;
            Height = 75;
            Center();
            Top = manager.ScreenHeight - 350;
            CloseButtonVisible = false;
            Resizable = false;

            MessageLabel = new Label(manager);
            MessageLabel.Init();
            MessageLabel.Text = Message;
            MessageLabel.Width = 300;
            MessageLabel.Height = 25;
            MessageLabel.Top = 10;
            MessageLabel.Left = 10;
            MessageLabel.Parent = this;
            Add(MessageLabel);
        }

        public void setTitle(String Title)
        {
            this.Text = Title;
        }

        public void setMessage(String Message)
        {
            this.MessageLabel.Text = Message;
        }
    }
}
