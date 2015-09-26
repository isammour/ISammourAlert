using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Foundation;

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
            if(_alertType == AlertType.Custom)
            {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            viewsList.Add(view);
            }
        }
        public void AddButton(string title,EventHandler buttonClicked,int fontSize)
        {
                var button = new UIButton(UIButtonType.RoundedRect);
                button.SetTitle(title ?? "Cancel", UIControlState.Normal);
                button.SetTitleColor(UIColor.White, UIControlState.Normal);
                button.BackgroundColor = UIColor.FromRGB(1f, 0.25f, 0.51f);
                //button.Layer.CornerRadius = 5f;
                button.Layer.BorderColor = UIColor.Gray.CGColor;
                button.Layer.BorderWidth = 1f;
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
            
            Add(title);
            Add(message);
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
        private void AddCustomViews()
        {
            foreach(var view in viewsList)
            {
                Add(view);
            }
            
        }
        private void AddButtons()
        {
            foreach (var button in buttonsList)
            {
                Add(button);
                AddConstraint(NSLayoutConstraint.Create(button, NSLayoutAttribute.Height, NSLayoutRelation.Equal, button, NSLayoutAttribute.Height, 1f, 20f));
            }
        }
        private void LayoutCustomAlert()
        {
            if (viewsList.Count != 0)
            {
                AddConstraint(NSLayoutConstraint.Create(viewsList[0], NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 10f));
                for (int i = 0; i < viewsList.Count; i++)
                {
                    AddConstraint(NSLayoutConstraint.Create(viewsList[i], NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
                    AddConstraint(NSLayoutConstraint.Create(viewsList[i], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));
                    if (i != 0)
                    {
                        AddConstraint(NSLayoutConstraint.Create(viewsList[i], NSLayoutAttribute.Top, NSLayoutRelation.Equal, viewsList[i - 1], NSLayoutAttribute.Bottom, 1f, 10f));
                    }
                }
            }
        }
        private void LayoutAlertButtons()
        {
            var lastElementIndex = buttonsList.Count - 1;
            if (buttonsList.Count != 0)
            {
                if(lastElementIndex == 0)
                {
                    AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0f));
                    AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
                    AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
                    return;
                }
                else if (buttonsList.Count %2 != 0 && lastElementIndex !=0)
                {
                    AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0f));
                    AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
                    AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
                    for(int i = lastElementIndex - 1; i >= 0; i--)
                    {
                        if(i%2 != 0)
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1f, 0f));
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.5f, 0f));
                        }
                        else if (i%2 == 0)
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Right, NSLayoutRelation.Equal, buttonsList[i + 1], NSLayoutAttribute.Left, 1f, 0f));
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.5f, 0f));
                        }

                        if (i + 2 > lastElementIndex)
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, buttonsList[lastElementIndex], NSLayoutAttribute.Top, 1f, 0f));
                        }
                        else
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, buttonsList[i + 2], NSLayoutAttribute.Top, 1f, 0f));
                        }
                    }
                    return;
                }
                else if(buttonsList.Count %2 ==0 && lastElementIndex != 0)
                {
                    for(int i = lastElementIndex; i >= 0; i--)
                    {
                        if(i == lastElementIndex)
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0f));
                        }
                        if (i % 2 != 0)
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1f, 0f));
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.5f, 0f));
                        }
                        else if (i % 2 == 0)
                        {
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Right, NSLayoutRelation.Equal, buttonsList[i + 1], NSLayoutAttribute.Left, 1f, 0f));
                            AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.5f, 0f));
                        }
                        if(i != lastElementIndex)
                        {
                            if (i + 2 > lastElementIndex)
                            {
                                AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0f));
                            }
                            else
                            {
                                AddConstraint(NSLayoutConstraint.Create(buttonsList[i], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, buttonsList[i + 2], NSLayoutAttribute.Top, 1f, 0f));
                            }
                        }
                    }
                    return;
                }
            }
        }
        private int CalculateHeight()
        {
            int height = 30;
            int buttonsHeight = 0;
            if (_alertType == AlertType.Normal)
            {
                height += (int)(message.Frame.Height + title.Frame.Height);
                foreach(var button in buttonsList)
                {
                    height +=(int)button.Frame.Height;
                }
                return height;
            }
            else if(_alertType == AlertType.Custom)
            {
                foreach(var view in viewsList)
                {
                    height += (int)view.Frame.Height + 10;
                }
                foreach(var button in buttonsList)
                {
                    buttonsHeight += (int)button.Frame.Height;
                }
                height += buttonsHeight / 2;
                return height;
            }
            return 0;
        }
        public void Show()
        {


            alertWindow = new UIWindow(UIScreen.MainScreen.Bounds);
            alertWindow.WindowLevel = UIWindowLevel.Alert;
            var controller = new UIViewController();

            controller.Add(this);

            var view = controller.View;

            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f));
            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f));
            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 0.9f, 0f));

            if (_alertType == AlertType.Normal)
            {
                AddButtons();
                CreateNormalAlert();
                LayoutNormalAlert();
                LayoutAlertButtons();
            }
            else if(_alertType == AlertType.Custom)
            {
                AddCustomViews();
                AddButtons();
                LayoutCustomAlert();
                LayoutAlertButtons();
            }
            alertWindow.RootViewController = controller;
            
            UIView.Animate(1, 0, UIViewAnimationOptions.TransitionFlipFromRight, () => {alertWindow.MakeKeyAndVisible() ; oldWindow.Alpha = 0.5f; }, null);
            int height = CalculateHeight();
            view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0f, height));
        }
        private void Dismiss(object sender, EventArgs ea)
        {
            oldWindow.MakeKeyWindow();

            var screenHeight = alertWindow.RootViewController.View.Frame.Height;
            var screenWidth = alertWindow.RootViewController.View.Frame.Width;
            UIView.Animate(1, 0, UIViewAnimationOptions.CurveEaseOut, () => { this.Center = new CGPoint(screenWidth/2, -100); oldWindow.Alpha = 1f;}, ()=> {
                alertWindow.Dispose();
                alertWindow.RemoveFromSuperview();
                alertWindow = null;
            });
        }
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            foreach(var view in viewsList)
            {
                if(view.IsFirstResponder)
                {
                    view.ResignFirstResponder();
                }
            }
        }
    }
}
