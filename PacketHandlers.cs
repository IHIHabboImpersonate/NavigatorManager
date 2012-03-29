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

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using IHI.Server;
using IHI.Server.Habbos;
using IHI.Server.Networking.Messages;
using NHibernate;
using NHibernate.Criterion;
using IHI.Server.Libraries.Cecer1.Navigator;
using LibNav = IHI.Server.Libraries.Cecer1.Navigator;

#endregion

namespace IHI.Server.Plugins.Cecer1.Navigator
{
    public partial class Navigator
    {
        private static void RegisterHandlers(object source, HabboEventArgs args)
        {
            Habbo target = source as Habbo;
            if (target == null)
                return;

            target
                .GetConnection()
                .AddHandler(150, PacketHandlerPriority.DefaultAction, ProcessRequestCategoryListings)
                .AddHandler(151, PacketHandlerPriority.DefaultAction, ProcessRequestUsableCategoryListing)
                .AddHandler(264, PacketHandlerPriority.DefaultAction, ProcessRequestRecommendedRoomListing)
                .AddHandler(16, PacketHandlerPriority.DefaultAction, ProcessRequestOwnRoomListing);
        }


        private static void ProcessRequestCategoryListings(Habbo sender, IncomingMessage message)
        {
            bool excludeFullRooms = message.PopWiredBoolean();
            int categoryID = message.PopWiredInt32();

            Category category = CoreManager.ServerCore.GetNavigator().GetCategory(categoryID);

            if (category == null)
                new MNavigatorCategoryListing // TODO: Remove this. Maybe even throw an exception?
                    {
                        ID = categoryID,
                        ExcludeFullRooms = excludeFullRooms,
                        Name = "Non-Existant Category",
                        ParentID = categoryID,
                        IsPublicCategory = true,
                        Listings = new Listing[0],
                        UnknownA = 0,
                        UnknownB = 10000,
                        UnknownC = 0
                    }.Send(sender);
            else
                new MNavigatorCategoryListing
                {
                    ID = categoryID,
                    ExcludeFullRooms = excludeFullRooms,
                    Name = category.Name,
                    ParentID = (category.PrimaryCategory != null ? category.PrimaryCategory.ID : category.ID),
                    IsPublicCategory = category.IsPublicCategory,
                    Listings = category.GetListings(),

                    UnknownA = 0,
                    UnknownB = 10000,
                    UnknownC = 0
                }.Send(sender);
        }

        private static void ProcessRequestUsableCategoryListing(Habbo sender, IncomingMessage message)
        {
            // TODO: Permissions

            LibNav.Navigator navigator = CoreManager.ServerCore.GetNavigator();

            new MUsableGuestCategoryListing
            {
                Categories = navigator.GetChildren(navigator.GuestRoot, NavigatorTreeSearchMode.GuestOnly)
            }.Send(sender);
        }

        private static void ProcessRequestRecommendedRoomListing(Habbo sender, IncomingMessage message)
        {
            // TODO: Recommended Room API methods

            new MRecommendedRoomListing
            {
                RoomListings = new GuestRoomListing[0] // No recommended rooms for now.
            }.Send(sender);
        }

        private static void ProcessRequestOwnRoomListing(Habbo sender, IncomingMessage message)
        {
            // TODO: Database tables for rooms.

            new MOwnRoomListing
            {
                Rooms = null
            }.Send(sender);
        }
    }
}