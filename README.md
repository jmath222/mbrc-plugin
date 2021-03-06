MusicBee Remote (Plugin)
====================

About
-------
This is a plugin for [MusicBee](http://getmusicbee.com/) that is required for [MusicBee Remote](https://github.com/kelsos/mbrc) android application to function.

The development version does a complete rework and redesign of the API, depracating the JSON data over TCP and moving to a web friendly implementation. The development version thus has a HTTP REST API (though you may find that this is not a pure REST implementation) along with websockets for communication an message pushing to the client.

If you want to check the currently released plugin code please check the mb22-fixes branch.

Currently there is no public documentation of the API Available but it is planned as soon as the version 1.x features are finalized, however there is a metadata page available when the plugin running that has information on most of the calls. The API is somewhat documented and build with ServiceStack.Api.Swagger, so if you put the *swagger-ui* folder in the MusicBee Plugins folder you should be able to access the documentation through *http://localhost:port/swagger-ui/index.html* (where port is the port marked as http at the plugin settings page). Please keep in mind that the resource list should be available under *http://localhost:port/resources*, and you will probably need to insert it manually if you downloaded and extracted the swagger-ui zip file.

Building
-------
To build the plugin you have to open it with Visual Studio 2015. After opening the project you will probably have to restore the required packages with NuGet.

Credits
-------

*  [ServiceStack v3](https://github.com/ServiceStackV3/ServiceStackV3)

    ServiceStack.Text is the library used for JSON serialization and deserialization.

    ServiceStack.OrmLite.Sqlite is used for the internal cache.

    *Plans are to use a fork of the ServiceStack.OrmLite.Sqlite library in order to add support for the latest version of System.Data.SQLite and use an newer version of SQLite*


    [BSD LICENCE](https://github.com/ServiceStack/ServiceStack/blob/v3/LICENSE)

*   [SQLite](https://www.sqlite.org/)

    [Public Domain](https://www.sqlite.org/copyright.html)

*   [NLOG](https://github.com/NLog/NLog)

    [BSD LICENCE](https://github.com/NLog/NLog/blob/master/LICENSE.txt)

*   [Ninject](https://github.com/ninject/ninject)

    [Apache v2](https://github.com/ninject/Ninject/blob/master/LICENSE.txt)


License
------


    MusicBee Remote (Plugin for MusicBee)
    Copyright (C) 2011-2015  Konstantinos Paparas

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
