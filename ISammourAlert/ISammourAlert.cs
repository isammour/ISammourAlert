using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace ISammourAlert
{
    public class ISammourAlert : UIView
    {
        public string Title { get; set; } = "Title";
        public string Message { get; set; } = "Message";
        public string ButtonTitle { get; set; } = "Dismiss";
        private UILabel title, message;
        private UIButton normalButton;
        private AlertType _alertType;
        private UIWindow oldWindow,alertWindow;
        private List<UIView> viewsList;
        private List<UIButton> buttonsList;

        public ISammourAlert(AlertType alertType)
        {
            _alertType = alertType;

            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.White;
            oldWindow = UIApplication.SharedApplication.KeyWindow;
            if (oldWindow == null)
            {
                Console.WriteLine("Move window.rootViewController = yourViewController below window.makeKeyAndVisible");
                return;
            }

            viewsList = new List<UIView>();
            buttonsList = new List<UIButton>();
        }

        public UILabel CreateLabel(string text, int fontSize, bool bold,UITextAlignment textAlignment)
        {
            var label = new UILabel();
            label.Text = text ?? "Title";
            if (bold)
            {
                label.Font = UIFont.BoldSystemFontOfSize(fontSize);
            }
            else
            {
                label.Font = UIFont.SystemFontOfSize(fontSize);
            }
            label.TextAlignment = textAlignment;
            label.SizeToFit();
            label.TranslatesAutoresizingMaskIntoConstraints = false;
            return label;
        }
        public UITextField CreateTextField(string placeholder, int fontSize , int maxRange , TextFieldStyle style)
        {
            var myTextField = new UITextField();
            if (style == TextFieldStyle.AlphaNumeric)
            {
                myTextField.KeyboardType = UIKeyboardType.Default;
            }
            else if (style == TextFieldStyle.Numeric)
            {
                myTextField.KeyboardType = UIKeyboardType.NumberPad;
                if (maxRange != 0)
                {
                    myTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
                    {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= maxRange;
                    };
                }

            }
            myTextField.BorderStyle = UITextBorderStyle.RoundedRect;
            myTextField.Layer.BorderWidth = 1f;
            myTextField.Layer.CornerRadius = 3f;
            myTextField.Placeholder = placeholder ?? "";
            myTextField.BackgroundColor = UIColor.White;
            myTextField.Font = UIFont.SystemFontOfSize(fontSize);
            myTextField.TranslatesAutoresizingMaskIntoConstraints = false;
            return myTextField;
        }
        public void AddView(UIView view)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            viewsList.Add(view);
        }
        public void AddButton(string title,EventHandler buttonClicked,int fontSize)
        {
            var button = new UIButton(UIButtonType.RoundedRect);
            button.SetTitle(title ?? "Cancel", UIControlState.Normal);
            button.SetTitleColor(UIColor.Black, UIControlState.Normal);
            button.BackgroundColor = UIColor.FromRGB(0.89f, 0.71f, 0.71f);
            button.Layer.CornerRadius = 5f;
            button.Font = UIFont.SystemFontOfSize(fontSize);
            button.TouchUpInside += buttonClicked ?? Dismiss;
            button.TranslatesAutoresizingMaskIntoConstraints = false;
            buttonsList.Add(button);
        }
        private void CreateNormalAlert()
        {
            title = CreateLabel(Title,20,true,UITextAlignment.Center);
            message = CreateLabel(Message, 18, false, UITextAlignment.Left);
            message.LineBreakMode = UILineBreakMode.WordWrap;
            message.Lines = 0;

            normalButton = new UIButton(UIButtonType.RoundedRect);
            normalButton.SetTitle(ButtonTitle, UIControlState.Normal);
            normalButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            normalButton.BackgroundColor = UIColor.FromRGB(0.89f, 0.71f, 0.71f);
            normalButton.Layer.BorderWidth = 1f;
            normalButton.Layer.BorderColor = UIColor.Gray.CGColor;
            normalButton.Font = UIFont.SystemFontOfSize(18);
            normalButton.TranslatesAutoresizingMaskIntoConstraints = false;
            normalButton.TouchUpInside += Dismiss;

            Add(title);
            Add(message);
            Add(normalButton);
        }
        private void LayoutNormalAlert()
        {
            //CenterX
            AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX,1f,0f));
            AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX,1f,0f));
            AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));

            //Title
            AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 10f));
            AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));

            //Message
            AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.Top, NSLayoutRelation.Equal, title, NSLayoutAttribute.Bottom, 1f, 10f));
            AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));

            //Button
            AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 5f));
            AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
            //AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 1f, 20f));
        }
        private void LayoutCustomAlert()
        {
        }
        private int CalculateHeight()
        {
            if(_alertType == AlertType.Normal)
            {
                return 30 + (int)(message.Frame.Height + title.Frame.Height + normalButton.Frame.Height);
            }
            return 0;
        }
        public void Show()
        {
            alertWindow = new UIWindow(UIScreen.MainScreen.Bounds);
            var controller = new UIViewController();

            controller.Add(this);

            var view = controller.View;

            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f));
            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f));
            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 0.9f, 0f));

            if (_alertType == AlertType.Normal)
            {
            CreateNormalAlert();
            LayoutNormalAlert();
            }

            alertWindow.RootViewController = controller;
            alertWindow.MakeKeyAndVisible();

            int height = CalculateHeight();
            
            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0f, height));
        }
        private void Dismiss(object sender, EventArgs ea)
        {
            oldWindow.MakeKeyWindow();

            var screenHeight = alertWindow.RootViewController.View.Frame.Height;
            var screenWidth = alertWindow.RootViewController.View.Frame.Width;
            UIView.Animate(2, 0, UIViewAnimationOptions.CurveEaseIn, () => { this.Center = new CGPoint(screenWidth/2, -100); }, ()=> {
                alertWindow.Dispose();
                alertWindow.RemoveFromSuperview();
                alertWindow = null;
            });
            
        }
    }
}
