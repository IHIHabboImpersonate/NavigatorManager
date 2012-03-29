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
    public class MRecommendedRoomListing : OutgoingMessage
    {
        public ICollection<GuestRoomListing> RoomListings { get; set; }

        public override OutgoingMessage Send(IMessageable target)
        {
            if (InternalOutgoingMessage.ID == 0)
            {
                InternalOutgoingMessage.Initialize(351)
                    .AppendInt32(RoomListings.Count);


                foreach (GuestRoomListing roomListing in RoomListings)
                {
                    InternalOutgoingMessage
                        .AppendInt32(roomListing.ID)
                        .AppendString(roomListing.Name)
                        .AppendString(roomListing.Owner.GetDisplayName());

                    switch (roomListing.LockMode)
                    {
                        case RoomLock.Open:
                            InternalOutgoingMessage.AppendString("open");
                            break;
                        case RoomLock.Password:
                            InternalOutgoingMessage.AppendString("password");
                            break;
                        case RoomLock.Doorbell:
                            InternalOutgoingMessage.AppendString("closed");
                            break;
                    }

                    InternalOutgoingMessage
                        .AppendInt32(roomListing.Population)
                        .AppendInt32(roomListing.Capacity)
                        .AppendString(roomListing.Description);
                }
            }
            target.SendMessage(InternalOutgoingMessage);
            return this;
        }
    }
}