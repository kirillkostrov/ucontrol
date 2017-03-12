
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using uControlAndroid.Entities;
using uControlAndroid.Common;
using uControlAndroid.Services;

namespace uControlAndroid
{
	[Activity(Label = "ControlsBuilderActivity", Theme = "@android:style/Theme.NoTitleBar")]
    public class ControlsBuilderActivity : Activity
    {
        GamePadService gamepadService;

        Button addGamepadBtn;
        Button showGamepadsListBtn;
        Button controlButtonBtn;
		Button controlStickBtn;
		Button controlToggleBtn;
        EditText gamepadNameInput;
		View controlsArea;

		GamePad[] gamePadsList;
        GamePad currentGamepad;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ControlsBuilderLayout);
            gamepadService = new GamePadService();
            gamePadsList = gamepadService.GetGamePadList();

            addGamepadBtn = FindViewById<Button>(Resource.Id.addGamepadBtn);
            showGamepadsListBtn = FindViewById<Button>(Resource.Id.showGamepadsListBtn);
            controlButtonBtn = FindViewById<Button>(Resource.Id.controlButtonBtn);
		    controlStickBtn = FindViewById<Button>(Resource.Id.controlStickBtn);
			controlToggleBtn = FindViewById<Button>(Resource.Id.controlToggleBtn);
			controlsArea = FindViewById<View>(Resource.Id.controlsArea);
            gamepadNameInput = FindViewById<EditText>(Resource.Id.gamepadNameInput);

            addGamepadBtn.Click += CreateNewGamepad;
            controlsArea.Drag += ButtonDragOnPlacement;
            controlButtonBtn.LongClick += (object sender, View.LongClickEventArgs e) => {
                var data = ClipData.NewPlainText("Type", ControlType.ActionButton.ToString());
				((sender) as Button).StartDrag(data, new View.DragShadowBuilder(((sender) as Button)), null, 0);
            };
			controlStickBtn.LongClick += (object sender, View.LongClickEventArgs e) =>
			{
                var data = ClipData.NewPlainText("Type", ControlType.Stick.ToString());
				((sender) as Button).StartDrag(data, new View.DragShadowBuilder(((sender) as Button)), null, 0);
			};
			controlToggleBtn.LongClick += (object sender, View.LongClickEventArgs e) =>
			{
                var data = ClipData.NewPlainText("Type", ControlType.Toggle.ToString());
				((sender) as Button).StartDrag(data, new View.DragShadowBuilder(((sender) as Button)), null, 0);
			};

        }

		private void ButtonDragOnPlacement(object sender, View.DragEventArgs e)
		{
			var evt = e.Event;
			switch (evt.Action)
			{
				case DragAction.Ended:
				case DragAction.Started:
					e.Handled = true;
					break;

				case DragAction.Drop:
					e.Handled = true;

                    var typeInString = e.Event.ClipData.GetItemAt(0).Text;
                    var posX = e.Event.GetX();
                    var posY = e.Event.GetY();
                    CreateNewControl(typeInString, posX, posY);
					break;
			}
		}

        private void CreateNewGamepad(object sender, EventArgs e) 
        {
            currentGamepad = new GamePad();

            showGamepadsListBtn.Visibility = ViewStates.Gone;
            gamepadNameInput.Visibility = ViewStates.Visible;
            gamepadNameInput.Text = "NewGamepad";
            addGamepadBtn.Text = "Save";
            addGamepadBtn.Click -= CreateNewGamepad;
            addGamepadBtn.Click += SaveNewGamepad;
			ToggleControlsButtons(ViewStates.Visible);
        }

		private void SaveNewGamepad(object sender, EventArgs e)
        {
            currentGamepad.Name = gamepadNameInput.Text;
			currentGamepad = gamepadService.SaveGamePad(currentGamepad);
            gamePadsList = gamepadService.GetGamePadList();

			addGamepadBtn.Text = "+ Create";
			addGamepadBtn.Click -= SaveNewGamepad;
			addGamepadBtn.Click += CreateNewGamepad;

            showGamepadsListBtn.Visibility = ViewStates.Visible;
            gamepadNameInput.Visibility = ViewStates.Gone;
            ToggleControlsButtons(ViewStates.Gone);
        }

        private void CreateNewControl(string type, float x, float y)
        {
            var control = new Control()
            {
                XPos = x,
                YPos = y,
                ControlType = (ControlType)Enum.Parse(typeof(ControlType), type),
            };
            var list = currentGamepad.Controls.ToList();
            list.Add(control);
            currentGamepad.Controls = list.ToArray();
        }

        private void ToggleControlsButtons(ViewStates state) 
        {
            controlButtonBtn.Visibility = state;
            controlStickBtn.Visibility = state;
            controlToggleBtn.Visibility = state;
        }
    }
}
