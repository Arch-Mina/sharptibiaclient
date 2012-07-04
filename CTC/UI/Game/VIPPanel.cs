﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CTC
{
    public class VIPPanel : UIVirtualFrame
    {
        ClientViewport Viewport;

        public VIPPanel()
        {
            Name = "VIP List";
            Padding = new Margin(5);
            ContentView = new UIStackView(UIStackDirection.Vertical);
            
            Bounds.Width = 200;
            Bounds.Height = 200;
        }

        public void OnNewState(ClientState NewState)
        {
            if (Viewport != null)
                Viewport.VIPStatusChanged -= VIPStatusChanged;

            Viewport = NewState.Viewport;
            UpdateList();

            Viewport.VIPStatusChanged += VIPStatusChanged;
        }

        private void VIPStatusChanged(ClientViewport Viewport, ClientCreature Creature)
        {
            if (Viewport != this.Viewport)
                return;

            UpdateList();
        }

        private void UpdateList()
        {
            ContentView.RemoveAllSubviews();

            List<ClientCreature> VIPs = new List<ClientCreature>(Viewport.VIPList.Values);
            VIPs.Sort(delegate(ClientCreature A, ClientCreature B)
            {
                if (A.Online && !B.Online)
                    return -1;
                else if (B.Online && !A.Online)
                    return 1;
                return A.Name.CompareTo(B.Name);
            });

            foreach (ClientCreature VIP in VIPs)
            {
                UILabel Label = new UILabel(VIP.Name);
                Label.Bounds.Width = ClientBounds.Width;
                Label.TextColor = VIP.Online ? Color.LightGreen : Color.Red;
                ContentView.AddSubview(Label);
            }
        }
        
        /*
        protected override void DrawContent(SpriteBatch CurrentBatch)
        {
            Vector2 pos = new Vector2(0, 0);
            foreach (ClientCreature vip in Viewport.VIPList.Values)
            {
                if (vip.Online == false)
                    continue;

                CurrentBatch.DrawString(
                    Context.StandardFont, vip.Name, ScreenCoordinate(pos),
                    (vip.Online ? Color.LightGreen : Color.Red),
                    0.0f, new Vector2(0.0f, 0.0f),
                    1.0f, SpriteEffects.None, 0.5f
                );

                pos.Y += 16;
            }
        }
        */
    }
}
