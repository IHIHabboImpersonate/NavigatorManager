#region GPLv3

// 
// Copyright (C) 2012  Chris Chenery
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

using System.Collections.Generic;
using IHI.Server.Libraries.Cecer1.Navigator;

namespace IHI.Server.Networking.Messages
{
    public class MOwnRoomListing : OutgoingMessage
    {
        public ICollection<GuestRoomListing> Rooms { get; set; }

        public override OutgoingMessage Send(IMessageable target)
        {
            if (InternalOutgoingMessage.ID == 0)
            {
                InternalOutgoingMessage.Initialize(0);
                // TODO: Finish this.
                // (What does packet 57 (@y) do?

                //// if (dTable.Rows.Count > 0)
                //// {
                ////     StringBuilder Rooms = new StringBuilder();
                ////     foreach (DataRow dRow in dTable.Rows)
                ////         Rooms.Append(Encoding.encodeVL64(Convert.ToInt32(dRow["id"])) + Convert.ToString(dRow["name"]) + Convert.ToChar(2) + _Username + Convert.ToChar(2) + roomManager.getRoomState(Convert.ToInt32(dRow["state"])) + Convert.ToChar(2) + Encoding.encodeVL64(Convert.ToInt32(dRow["visitors_now"])) + Encoding.encodeVL64(Convert.ToInt32(dRow["visitors_max"])) + Convert.ToString(dRow["description"]) + Convert.ToChar(2));
                ////     sendData("@P" + Encoding.encodeVL64(dTable.Rows.Count) + Rooms.ToString());
                //// }
                //// else
                //// {
                ////     sendData("@y" + _Username);
                //// }
            }
            target.SendMessage(InternalOutgoingMessage);
            return this;
        }
    }
}