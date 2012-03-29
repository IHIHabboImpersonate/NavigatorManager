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
    public class MUsableGuestCategoryListing : OutgoingMessage
    {
        public ICollection<Category> Categories { get; set; }

        public override OutgoingMessage Send(IMessageable target)
        {
            if (InternalOutgoingMessage.ID == 0)
            {
                InternalOutgoingMessage.Initialize(221)
                    .AppendInt32(Categories.Count);

                foreach (Category category in Categories)
                {
                    InternalOutgoingMessage
                        .AppendInt32(category.ID)
                        .AppendString(category.Name);
                    continue;
                }
            }
            target.SendMessage(InternalOutgoingMessage);
            return this;
        }
    }
}