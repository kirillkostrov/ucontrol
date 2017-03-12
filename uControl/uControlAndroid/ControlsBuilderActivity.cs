
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
using Entities;

namespace uControlAndroid
{
	[Activity(Label = "ControlsBuilderActivity", Theme = "@android:style/Theme.NoTitleBar")]
    public class ControlsBuilderActivity : Activity
    {
        Button addGamepadBtn;
        Button showGamepadsListBtn;
        Button controlButtonBtn;
		Button controlStickBtn;
		Button controlToggleBtn;
        EditText gamepadNameInput;
		View controlsArea;

		List<GamePad> gamePadsList;
        GamePad currentGamepad;
        Control currentControl;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ControlsBuilderLayout);

            addGamepadBtn = FindViewById<Button>(Resource.Id.addGamepadBtn);
            showGamepadsListBtn = FindViewById<Button>(Resource.Id.showGamepadsListBtn);
            controlButtonBtn = FindViewById<Button>(Resource.Id.controlButtonBtn);
		    controlStickBtn = FindViewById<Button>(Resource.Id.controlStickBtn);
			controlToggleBtn = FindViewById<Button>(Resource.Id.controlToggleBtn);
			controlsArea = FindViewById<View>(Resource.Id.controlsArea);

            addGamepadBtn.Click += AddNewGamepad;
            controlsArea.Drag += ButtonDragOnPlacement;
            controlButtonBtn.LongClick += (object sender, View.LongClickEventArgs e) => {
                var data = ClipData.NewPlainText("Type", ControlType.Button.ToString());
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
					break;
			}
		}

        private void AddNewGamepad(object sender, EventArgs e) 
        {
            if (gamePadsList == null)
            {
                gamePadsList = new List<GamePad>();
            }
			var gamePad = new GamePad();
            gamePad.Id = gamePadsList.Count();
            //gamePad.
        }
    }
}
