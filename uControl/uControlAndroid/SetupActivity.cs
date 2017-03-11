﻿using Android.App;
using Android.OS;
using Android.Widget;

namespace Andrule
{

    [Activity]
    public class SetupActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TextView textview = new TextView(this) {Text = "This is the SetupActivity tab" };
            SetContentView(textview);
        }
    }
}