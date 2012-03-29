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
using System.Linq;

namespace IHI.Server.Networking.Messages
{
    public class MNavigatorCategoryListing : OutgoingMessage
    {
        public int ID { get; set; }
        public bool IsPublicCategory { get; set; }
        public string Name { get; set; }
        public int UnknownA { get; set; } // TODO: Find out the meaning of these values.
        public int UnknownB { get; set; }
        public int UnknownC { get; set; }
        public int ParentID { get; set; }

        public bool ExcludeFullRooms { get; set; }

        public ICollection<Listing> Listings { get; set; }

        public override OutgoingMessage Send(IMessageable target)
        {
            if (InternalOutgoingMessage.ID == 0)
            {
                InternalOutgoingMessage.Initialize(220)
                    .AppendBoolean(ExcludeFullRooms)
                    .AppendInt32(ID)
                    .AppendInt32(IsPublicCategory ? 0 : 2)
                    .AppendString(Name)
                    .AppendInt32(UnknownA)
                    .AppendInt32(UnknownB)
                    .AppendInt32(ParentID);

                if(!IsPublicCategory)
                    InternalOutgoingMessage
                        .AppendInt32(
                            Listings
                                .Where(listing => listing is GuestRoomListing)
                                .Count());

                foreach (Listing listing in Listings)
                {
                    if (listing is Category)
                    {
                        InternalOutgoingMessage
                            .AppendInt32(listing.ID)
                            .AppendBoolean(false)
                            .AppendString(listing.Name)
                            .AppendInt32(listing.Population)
                            .AppendInt32(listing.Capacity)
                            .AppendInt32(ID);
                        continue;
                    }

                    if (listing is GuestRoomListing)
                    {
                        GuestRoomListing specificListing = listing as GuestRoomListing;

                        InternalOutgoingMessage
                            .AppendInt32(listing.ID)
                            .AppendString(listing.Name)
                            .AppendString(specificListing.Owner.GetDisplayName());

                        switch (specificListing.LockMode)
                        {
                            case RoomLock.Open:
                                {
                                    InternalOutgoingMessage.AppendString("open");
                                    break;
                                }
                            case RoomLock.Password:
                                {
                                    InternalOutgoingMessage.AppendString("password");
                                    break;
                                }
                            case RoomLock.Doorbell:
                                {
                                    InternalOutgoingMessage.AppendString("closed");
                                    break;
                                }
                        }
                        InternalOutgoingMessage
                            .AppendInt32(listing.Population)
                            .AppendInt32(listing.Capacity)
                            .AppendString(specificListing.Description);
                        continue;
                    }


                    if (listing is PublicRoomListing)
                    {
                        PublicRoomListing specificListing = listing as PublicRoomListing;

                        InternalOutgoingMessage
                            .AppendInt32(listing.ID)
                            .AppendBoolean(true)
                            .AppendString(listing.Name)
                            .AppendInt32(listing.Population)
                            .AppendInt32(listing.Capacity)
                            // Possible category ID needed here?
                            .AppendString(specificListing.Description)
                            .AppendInt32(listing.ID)
                            .AppendInt32(UnknownA)
                            .AppendString(specificListing.ClientFiles)
                            .AppendInt32(UnknownB)
                            .AppendInt32(UnknownC);
                        continue;
                    }
                }
            }
            target.SendMessage(InternalOutgoingMessage);
            return this;
        }
    }
}