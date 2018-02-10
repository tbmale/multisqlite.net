# multisqlite.net

_WIP_

System.Data.Sqlite overload for allowing connection to a group of identical structure database files.
When a database file exceeds the size declared with the connection parameter __slice size__, a new database file with the same structure is added to the group.
The modification queries are performed on the last database file in group.

Only the __ExecuteReader__ and __ExecuteNonQuery__ methods are currently implemented in the _mSQLiteCommand_ class.
The __slice size__ parameter accepts "human readable" sizes (ex. `1Gb`, `3.14MB`, `0.12T`, ...).
