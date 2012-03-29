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

namespace IHI.Server.Plugins.Cecer1.Navigator
{
    public static class ExtensionMethods
    {
        private static Libraries.Cecer1.Navigator.Navigator _navigatorInstance;

        public static Libraries.Cecer1.Navigator.Navigator GetNavigator(this ServerCore instance)
        {
            return _navigatorInstance;
        }

        public static ServerCore SetNavigator(this ServerCore instance, Libraries.Cecer1.Navigator.Navigator navigatorInstance)
        {
            _navigatorInstance = navigatorInstance;
            return instance;
        }
    }
}