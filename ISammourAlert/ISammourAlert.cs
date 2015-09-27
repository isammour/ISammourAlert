using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace ISammourAlert
{
<<<<<<< HEAD
    public class ISammourAlert : UIView
    {
        public string Title { get; set; } = "Title";
        public string Message { get; set; } = "Message";
        public string ButtonTitle { get; set; } = "Dismiss";
        private UILabel title, message;
        private AlertType _alertType;
        private UIButton normalButton;
        private UIWindow oldWindow,alertWindow;
        private List<UIView> viewsList;
        private List<UIButton> buttonsList;
        private NSLayoutConstraint  centerY,centerX;
        private UIView view;
        private AnimationType _animationType;


        public ISammourAlert(AlertType alertType,AnimationType animationType)
        {
            _alertType = alertType;
            _animationType = animationType;

            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.White;
            Layer.CornerRadius = 5f;
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
                button.BackgroundColor = UIColor.FromRGB(0.56f,0.79f,0.98f);
                //button.Layer.CornerRadius = 5f;
                button.Layer.BorderColor = UIColor.Gray.CGColor;
                button.Layer.BorderWidth = 0.5f;
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
            //AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));

            //Title
            AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 10f));
            AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));

            //Message
            AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.Top, NSLayoutRelation.Equal, title, NSLayoutAttribute.Bottom, 1f, 10f));
            AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));

            //Button
            //AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 5f));
            //AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
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
                    buttonsHeight +=(int)button.Frame.Height;
                }
                if(buttonsList.Count%2 != 0 && buttonsList.Count != 0)
                {
                    height += (int)buttonsList[0].Frame.Height;
                }
                height += buttonsHeight / 2;
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
                if (buttonsList.Count % 2 == 0 && buttonsList.Count != 0)
                {
                    height += (int)buttonsList[0].Frame.Height;
                }
                height += buttonsHeight / 2;
                return height;
            }
            return 0;
        }
        public void Show()
        {
            if(alertWindow == null)
            {
                alertWindow = new UIWindow(UIScreen.MainScreen.Bounds);
                alertWindow.WindowLevel = UIWindowLevel.StatusBar;
                var controller = new UIViewController();
                Hidden = true;
                controller.Add(this);

                view = controller.View;

                AnimationInitialPositions();

                view.AddConstraint(centerX);
                view.AddConstraint(centerY);
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

                alertWindow.MakeKeyAndVisible();
                view.LayoutIfNeeded();
                int height = CalculateHeight();
                view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0f, height));

            }
            view = alertWindow.RootViewController.View;
            alertWindow.Hidden = false;
            OnAnimationShow();
            UIView.Animate(1, 0, UIViewAnimationOptions.TransitionFlipFromRight, () => {view.LayoutIfNeeded();  Hidden = false; oldWindow.Alpha = 0.5f; }, null);
            Console.WriteLine(title.Frame.Height);
            Console.WriteLine(message.Frame.Height);
            foreach(var btn in buttonsList)
            {
                Console.WriteLine(btn.Frame.Height);
            }
            Console.WriteLine(this.Frame.Height);
        }
        private void AnimationInitialPositions()
        {
            var screenHeight = UIScreen.MainScreen.Bounds.Height * 2;
            var screenWidth = UIScreen.MainScreen.Bounds.Width;
            //Setting start positions
            if (_animationType == AnimationType.TopToCenter)
            {
                centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f);
                centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, -1000);
            }
            if (_animationType == AnimationType.RightToCenter)
            {
                centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 1000);
                centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f);
            }

            if (_animationType == AnimationType.LeftToCenter)
            {
                centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, -1000);
                centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f);
            }
            if (_animationType == AnimationType.BottomToCenter)
            {
                centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f);
                centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 1000);
            }
        }
        private void OnAnimationShow()
        {
            
            if(_animationType == AnimationType.TopToCenter)
            {
                centerY.Constant = 0;
            }
            if(_animationType == AnimationType.RightToCenter)
            {
                centerX.Constant = 0;
            }
            if(_animationType == AnimationType.LeftToCenter)
            {
                centerX.Constant = 0;
            }
            if(_animationType == AnimationType.BottomToCenter)
            {
                centerY.Constant = 0;
            }
        }
        private void OnAnimationDismiss()
        {
            if (_animationType == AnimationType.TopToCenter)
            {
                centerY.Constant = -1000f ;
            }
            if (_animationType == AnimationType.RightToCenter)
            {
                centerX.Constant = 1000f;
            }
            if (_animationType == AnimationType.LeftToCenter)
            {
                centerX.Constant = -1000f;
            }
            if (_animationType == AnimationType.BottomToCenter)
            {
                centerY.Constant = 1000f;
            }
        }
        private void Dismiss(object sender, EventArgs ea)
        {

            var screenHeight = alertWindow.RootViewController.View.Frame.Height;
            var screenWidth = alertWindow.RootViewController.View.Frame.Width;
            OnAnimationDismiss();
            UIView.Animate(1, 0, UIViewAnimationOptions.CurveEaseOut, () => { view.LayoutIfNeeded(); oldWindow.Alpha = 1f;},()=> {
                Hidden = true;
                alertWindow.Hidden = true;
                oldWindow.MakeKeyWindow();
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
=======
	public class ISammourAlert : UIView
	{
		private UILabel title, message;
		private AlertType _alertType;
		private UIButton normalButton;
		private UIWindow oldWindow, alertWindow;
		private List<UIView> viewsList;
		private List<UIButton> buttonsList;
		private NSLayoutConstraint centerY, centerX;
		private UIView view;
		private AnimationType _animationType;

		public ISammourAlert(AlertType alertType, AnimationType animationType)
		{
			_alertType = alertType;
			_animationType = animationType;

			TranslatesAutoresizingMaskIntoConstraints = false;
			BackgroundColor = UIColor.White;
			Layer.CornerRadius = 5f;
			oldWindow = UIApplication.SharedApplication.KeyWindow;
			if (oldWindow == null)
			{
				throw new InvalidOperationException("Move window.rootViewController = yourViewController below window.makeKeyAndVisible");
			}
			viewsList = new List<UIView>();
			buttonsList = new List<UIButton>();
		}

		public string Title { get; set; } = "Title";

		public string Message { get; set; } = "Message";

		public string ButtonTitle { get; set; } = "Dismiss";

		public UILabel CreateLabel(string text, int fontSize, bool bold, UITextAlignment textAlignment)
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

		public UITextField CreateTextField(string placeholder, int fontSize, int maxRange, TextFieldStyle style)
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
			if (_alertType == AlertType.Custom)
			{
				view.TranslatesAutoresizingMaskIntoConstraints = false;
				viewsList.Add(view);
			}
		}

		public void AddButton(string title, EventHandler buttonClicked, int fontSize)
		{
			var button = new UIButton(UIButtonType.RoundedRect);
			button.SetTitle(title ?? "Cancel", UIControlState.Normal);
			button.SetTitleColor(UIColor.White, UIControlState.Normal);
			button.BackgroundColor = UIColor.FromRGB(0.56f, 0.79f, 0.98f);
			//button.Layer.CornerRadius = 5f;
			button.Layer.BorderColor = UIColor.Gray.CGColor;
			button.Layer.BorderWidth = 0.5f;
			button.Font = UIFont.SystemFontOfSize(fontSize);
			button.TouchUpInside += buttonClicked ?? Dismiss;
			button.TranslatesAutoresizingMaskIntoConstraints = false;
			buttonsList.Add(button);
		}

		private void CreateNormalAlert()
		{
			title = CreateLabel(Title, 20, true, UITextAlignment.Center);
			message = CreateLabel(Message, 18, false, UITextAlignment.Left);
			message.LineBreakMode = UILineBreakMode.WordWrap;
			message.Lines = 0;

			Add(title);
			Add(message);
		}

		private void LayoutNormalAlert()
		{
			//CenterX
			AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
			AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
			//AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));

			//Title
			AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 10f));
			AddConstraint(NSLayoutConstraint.Create(title, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));

			//Message
			AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.Top, NSLayoutRelation.Equal, title, NSLayoutAttribute.Bottom, 1f, 10f));
			AddConstraint(NSLayoutConstraint.Create(message, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.9f, 0f));

			//Button
			//AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 5f));
			//AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
			//AddConstraint(NSLayoutConstraint.Create(normalButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 1f, 20f));
		}

		private void AddCustomViews()
		{
			foreach (var view in viewsList)
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
				if (lastElementIndex == 0)
				{
					AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0f));
					AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
					AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
					return;
				}
				else if (buttonsList.Count % 2 != 0 && lastElementIndex != 0)
				{
					AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0f));
					AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1f, 0f));
					AddConstraint(NSLayoutConstraint.Create(buttonsList[lastElementIndex], NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1f, 0f));
					for (int i = lastElementIndex - 1; i >= 0; i--)
					{
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
				else if (buttonsList.Count % 2 == 0 && lastElementIndex != 0)
				{
					for (int i = lastElementIndex; i >= 0; i--)
					{
						if (i == lastElementIndex)
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
						if (i != lastElementIndex)
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
				foreach (var button in buttonsList)
				{
					buttonsHeight += (int)button.Frame.Height;
				}
				if (buttonsList.Count % 2 != 0 && buttonsList.Count != 0)
				{
					height += (int)buttonsList[0].Frame.Height;
				}
				height += buttonsHeight / 2;
				return height;
			}
			else if (_alertType == AlertType.Custom)
			{
				foreach (var view in viewsList)
				{
					height += (int)view.Frame.Height + 10;
				}
				foreach (var button in buttonsList)
				{
					buttonsHeight += (int)button.Frame.Height;
				}
				if (buttonsList.Count % 2 == 0 && buttonsList.Count != 0)
				{
					height += (int)buttonsList[0].Frame.Height;
				}
				height += buttonsHeight / 2;
				return height;
			}
			return 0;
		}

		public void Show()
		{
			if (alertWindow == null)
			{
				alertWindow = new UIWindow(UIScreen.MainScreen.Bounds);
				alertWindow.WindowLevel = UIWindowLevel.StatusBar;
				var controller = new UIViewController();
				Hidden = true;
				controller.Add(this);

				view = controller.View;

				AnimationInitialPositions();

				view.AddConstraint(centerX);
				view.AddConstraint(centerY);
				view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 0.9f, 0f));

				if (_alertType == AlertType.Normal)
				{
					AddButtons();
					CreateNormalAlert();
					LayoutNormalAlert();
					LayoutAlertButtons();
				}
				else if (_alertType == AlertType.Custom)
				{
					AddCustomViews();
					AddButtons();
					LayoutCustomAlert();
					LayoutAlertButtons();
				}
				alertWindow.RootViewController = controller;

				alertWindow.MakeKeyAndVisible();
				int height = CalculateHeight();
				view.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0f, height));
			}
			view = alertWindow.RootViewController.View;
			alertWindow.Hidden = false;
			OnAnimationShow();
			UIView.Animate(1, 0, UIViewAnimationOptions.TransitionFlipFromRight, () => { view.LayoutIfNeeded(); Hidden = false; oldWindow.Alpha = 0.5f; }, null);
		}

		private void AnimationInitialPositions()
		{
			var screenHeight = UIScreen.MainScreen.Bounds.Height * 2;
			var screenWidth = UIScreen.MainScreen.Bounds.Width;
			//Setting start positions
			if (_animationType == AnimationType.TopToCenter)
			{
				centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f);
				centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, -1000);
			}
			if (_animationType == AnimationType.RightToCenter)
			{
				centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 1000);
				centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f);
			}

			if (_animationType == AnimationType.LeftToCenter)
			{
				centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, -1000);
				centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f);
			}
			if (_animationType == AnimationType.BottomToCenter)
			{
				centerX = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f);
				centerY = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 1000);
			}
		}

		private void OnAnimationShow()
		{
			if (_animationType == AnimationType.TopToCenter)
			{
				centerY.Constant = 0;
			}
			if (_animationType == AnimationType.RightToCenter)
			{
				centerX.Constant = 0;
			}
			if (_animationType == AnimationType.LeftToCenter)
			{
				centerX.Constant = 0;
			}
			if (_animationType == AnimationType.BottomToCenter)
			{
				centerY.Constant = 0;
			}
		}

		private void OnAnimationDismiss()
		{
			if (_animationType == AnimationType.TopToCenter)
			{
				centerY.Constant = -1000f;
			}
			if (_animationType == AnimationType.RightToCenter)
			{
				centerX.Constant = 1000f;
			}
			if (_animationType == AnimationType.LeftToCenter)
			{
				centerX.Constant = -1000f;
			}
			if (_animationType == AnimationType.BottomToCenter)
			{
				centerY.Constant = 1000f;
			}
		}

		private void Dismiss(object sender, EventArgs ea)
		{
			var screenHeight = alertWindow.RootViewController.View.Frame.Height;
			var screenWidth = alertWindow.RootViewController.View.Frame.Width;
			OnAnimationDismiss();
			UIView.Animate(1, 0, UIViewAnimationOptions.CurveEaseOut, () => { view.LayoutIfNeeded(); oldWindow.Alpha = 1f; }, () =>
			{
				Hidden = true;
				alertWindow.Hidden = true;
				oldWindow.MakeKeyWindow();
			});
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			foreach (var view in viewsList)
			{
				if (view.IsFirstResponder)
				{
					view.ResignFirstResponder();
				}
			}
		}
	}
}
>>>>>>> e384e1e104b3bc3eaf1b444c7b29df979163bcc8
