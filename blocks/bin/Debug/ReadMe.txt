Read me!!!!!!!!!!!!!!!!!!!!!
All your notes are stored in note<UserID>.xml file located in main folder.
You can drag and drop notepad and word files to aplication as well from aplication
to any PC location.
Both content, position, size and color are dynamically save durning performing any
changes in app.
By right click mouse you trigger context menu with cut, copy and paste option, and you can operate
on text within notes and any windows text app like Word etc.   
After drag and drop is small chance to lost note (f.e. note is out of aplication window (or nearly out))
then you can click "Refresh".
If you want to delete note - click "X" button.
To resize press and hold ">" button and move mouse cursor.
Header text of note is name of "yellow card" that is further use f.e. as file name.
If you leave blank header and drag and drop note to windows location, then filename will be:
"note12345", where "12345" is generated randomly. By default when you create new note name is "New note<NoteID>".
This NoteID is unique and every new note has larger number than previous - despite deleted positions.
If you delete note by mistake - before you close app copy note<UserID>.xml file to another location,
then close app and overwrite original file with this one.
Max file size can be 100MB. If you need to use larger files, send me e-mail with request.

By selecting date from calendar (and time) and clickig "Revoke" button you trigger timer (background 
color of this button will change from green to ren).
When selected date will be equal (or past) than your local time then you will see message box with informationn that
note with particular name was revoked. Above that you will hear notification sound (except you check "Disable Sound" option)
If you click ok on notification message box this will cancel notification. If you want to cancel notification manually - just select
past date and click "Revoke" button. Then color of this button change background color from red to green and you will see
message "You have selected a past date".

Important!!!
I highly recommend you to regular performing backup your notes.
By default backup is performing when you close app.
You can do this manually - just click "Perform BackUp". This option create back up of your all notes-
xml file in format currentDataAndTimeNote<UserID>.xml". To restore particular file  just click Tools- Open restoring point.
You will see list from some details about restoring points (date and notes count in particular restoring point).
Choose one from list and click Restore button. If you want to see mor detail about particular restoring point - go to 
BackUp folder and open proper file in Notepad or Notepad++ (recomended).
If you want to delete all old restoring points click Tools-Delete old backups. This option delete all backups except the newest.

In critical situation:
copy this file from BackUp folder to main and change name of this file to:
note<UserID>.xml
Please notice that in standalone version this file has often name: note1.xml


Requiments: min. 3.5 dotNET framework