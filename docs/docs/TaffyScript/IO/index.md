---
layout: default
title: TaffyScript.IO
---

# TaffyScript.IO

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/)

Provides access to scripts and objects related to processing input and output.

## Objects

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer">Buffer</a></td>
      <td>Represents an array of bytes that can be used to efficiently encode data.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo">DirectoryInfo</a></td>
      <td>Exposes scripts for creating, moving, and iterating through directories.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo">FileInfo</a></td>
      <td>Provides mechanisms for the creation, copying, deletion, moving, and opening of files.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream">FileStream</a></td>
      <td>Provides a Stream for a file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/MemoryStream">MemoryStream</a></td>
      <td>Creates a stream whose backing store is memory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader">StreamReader</a></td>
      <td>Reads characters from a byte stream using a particular encoding.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter">StreamWriter</a></td>
      <td>Implements a TextWriter for writing characters to a stream in a particular encoding.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringReader">StringReader</a></td>
      <td>Implements a TextReader that reads from a string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter">StringWriter</a></td>
      <td>Implements a TextWriter for writing information to a string. The information is stored in a [StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder).</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextReader">TextReader</a></td>
      <td>Represents a reader that can read a sequential series of characters.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter">TextWriter</a></td>
      <td>Represents a writer that can write a sequential series of characters.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream">Stream</a></td>
      <td>Base class for TaffyScript streams.</td>
    </tr>
  </tbody>
</table>

## Scripts

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Signature</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_create">directory_create(path)</a></td>
      <td>Creates a directory and all subdirectories in the specified path unless they already exist.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_delete">directory_delete(path, [recursive=false])</a></td>
      <td>Deletes the directory, and, if specified, any subdirectories and files.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_enumerate_directories">directory_enumerate_directories(path, [search_pattern], [search_option])</a></td>
      <td>Gets a collection of directory names that meet the specified criteria.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_enumerate_files">directory_enumerate_files(path, [search_pattern], [search_option])</a></td>
      <td>Gets a collection of file names that meet the specified criteria.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_enumerate_file_system_entries">directory_enumerate_file_system_entries(path, [search_pattern], [search_option])</a></td>
      <td>Gets a collection of file and directory names that meet the specified criteria.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_exists">directory_exists(path)</a></td>
      <td>Determines if a diretory with the specified path exists.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_creation_time">directory_get_creation_time(path)</a></td>
      <td>Gets the creation time of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_creation_time_utc">directory_get_creation_time_utc(path)</a></td>
      <td>Gets the creation time of a directory in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_current">directory_get_current()</a></td>
      <td>Gets the working directory of the application.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_directories">directory_get_directories(path, [search_pattern], [search_option])</a></td>
      <td>Gets an array of directory names that meet the specified criteria.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_files">directory_get_files(path, [search_pattern], [search_option])</a></td>
      <td>Gets an array of file names that meet the specified criteria.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_file_system_entries">directory_get_file_system_entries(path, [search_pattern], [search_option])</a></td>
      <td>Gets an array of file and directory names that meet the specified criteria.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_last_access_time">directory_get_last_access_time(path)</a></td>
      <td>Gets the last access time of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_last_access_time_utc">directory_get_last_access_time_utc(path)</a></td>
      <td>Gets the last access time of a directory in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_last_write_time">directory_get_last_write_time(path)</a></td>
      <td>Gets the last write time of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_last_write_time_utc">directory_get_last_write_time_utc(path)</a></td>
      <td>Gets the last write time of a directory in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_logical_drives">directory_get_logical_drives()</a></td>
      <td>Gets a list of logical drives on this computer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_root">directory_get_root(path)</a></td>
      <td>Gets the volume/root information of a path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_get_parent">directory_get_parent(path)</a></td>
      <td>Gets the parent directory of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_move">directory_move(source_directory, destination_directory)</a></td>
      <td>Moves a directory and its contents to a new location.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_creation_time">directory_set_creation_time(path, time)</a></td>
      <td>Sets the creation time of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_creation_time_utc">directory_set_creation_time_utc(path, time)</a></td>
      <td>Sets the creation time of a directory in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_current">directory_set_current(path)</a></td>
      <td>Sets the working directory of the application.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_last_access_time">directory_set_last_access_time(path, time)</a></td>
      <td>Sets the last access time of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_last_access_utc">directory_set_last_access_utc(path, time)</a></td>
      <td>Sets the last access time of a directory in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_last_write_time">directory_set_last_write_time(path, time)</a></td>
      <td>Sets the last write time of a directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/directory_set_last_write_time_utc">directory_set_last_write_time_utc(path, time)</a></td>
      <td>Sets the last write time of a directory in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_append_all_lines">file_append_all_lines(file_name, lines, [encoding])</a></td>
      <td>Appends the lines to a file and then closes the file. If the file does not exist, it is created first.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_append_all_text">file_append_all_text(file_name, text, [encoding])</a></td>
      <td>Appends the text to a file and then closes the file. If the file does not exist, it is created first.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_append_text">file_append_text(file_name)</a></td>
      <td>Creates a [StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter) that appends utf-8 encoded text to the file. If the file does not exist, it is created first.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_copy">file_copy(source_file, dest_file, [overwrite=false])</a></td>
      <td>Copies an existing file to a new file, optionally allowing the overwrite of an existing file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_create">file_create(file_name, [buffer_size], [options])</a></td>
      <td>Creates a file with the specified path and returns a FileStream for it.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_create_text">file_create_text(file_name)</a></td>
      <td>Creates or opens a file for writing utf-8 encoded text. If the file exists, its contents are overwritten.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_decrypt">file_decrypt(file_name)</a></td>
      <td>Decrypts a file that was encrypted by the current account.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_delete">file_delete(file_name)</a></td>
      <td>Deletes the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_encrypt">file_encrypt(file_name)</a></td>
      <td>Encrypts a file so that only the account used to encrypt it can decrypt it.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_exists">file_exists(file_name)</a></td>
      <td>Determines whether the specified file exists.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_attributes">file_get_attributes(file_name)</a></td>
      <td>Gets the attributes of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_creation_time">file_get_creation_time(file_name)</a></td>
      <td>Gets the creation time of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_creation_time_utc">file_get_creation_time_utc(file_name)</a></td>
      <td>Gets the creation time of the specified file in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_last_access_time">file_get_last_access_time(file_name)</a></td>
      <td>Gets the last access time of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_last_access_time_utc">file_get_last_access_time_utc(file_name)</a></td>
      <td>Gets the last access time of the specified file in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_last_write_time">file_get_last_write_time(file_name)</a></td>
      <td>Gets the last write time of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_get_last_write_time_utc">file_get_last_write_time_utc(file_name)</a></td>
      <td>Gets the last write time of the specified file in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_move">file_move(source_file, dest_file)</a></td>
      <td>Moves the specified file to a new location.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_open">file_open(path, mode, [access], [share])</a></td>
      <td>Opens a FileStream on the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_open_read">file_open_read(path)</a></td>
      <td>Opens an existing file for reading.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_open_text">file_open_text(path)</a></td>
      <td>Opens an existing utf-8 encoded text file for reading.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_open_write">file_open_write(path)</a></td>
      <td>Opens an existing file or creates a new file for writing.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_read_all_bytes">file_read_all_bytes(path)</a></td>
      <td>Opens a binary file, reads the contents into a Buffer, and then closes the file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_read_all_lines">file_read_all_lines(path, [encoding=*utf-8*])</a></td>
      <td>Opens a text file, reads all lines into an array, then closes the file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_read_all_text">file_read_all_text(path, [encoding=*utf-8*])</a></td>
      <td>Opens a text file, reads the contents into a string, then closes the file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_read_lines">file_read_lines(path, [encoding=*utf-8*])</a></td>
      <td>Reads the lines of a file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_replace">file_replace(source_file, dest_file, dest_backup_file, [ignore_metadata_errors=false])</a></td>
      <td>Replaces the contents of a file with the contents of another file, deleting the original file and creating a backup of the replaced file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_set_creation_time">file_set_creation_time(file_name, time)</a></td>
      <td>Sets the creation time of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_set_creation_time_utc">file_set_creation_time_utc(file_name, time)</a></td>
      <td>Sets the creation time of the specified file in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_set_last_access_time">file_set_last_access_time(file_name, time)</a></td>
      <td>Sets the last access time of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_set_last_access_time_utc">file_set_last_access_time_utc(file_name, time)</a></td>
      <td>Sets the last access time of the specified file in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_set_last_write_time">file_set_last_write_time(file_name, time)</a></td>
      <td>Sets the last write time of the specified file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_set_last_write_time_utc">file_set_last_write_time_utc(file_name, time)</a></td>
      <td>Sets the last write time of the specified file in universal coordinated time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_write_all_bytes">file_write_all_bytes(path, buffer)</a></td>
      <td>Creates a new file, writes the specified Buffer to the file, then closes the file. If the files exists, it is overwritten.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_write_all_lines">file_write_all_lines(path, lines, [encoding=*utf-8*])</a></td>
      <td>Creates a new file, writes one or more strings to the file, then closes the file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/file_write_all_text">file_write_all_text(path, content, [encoding=*utf-8*])</a></td>
      <td>Creates a new file, writes the contents to the file, then closes the file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_change_extension">path_change_extension(path, extension)</a></td>
      <td>Changes the extension of a path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_combine">path_combine(..paths)</a></td>
      <td>Combines the arguments into a single path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_directory_name">path_get_directory_name(path)</a></td>
      <td>Gets the directory name for the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_extension">path_get_extension(path)</a></td>
      <td>Gets the extension of the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_file_name">path_get_file_name(path)</a></td>
      <td>Gets the file name and extension of the the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_file_name_without_extension">path_get_file_name_without_extension(path)</a></td>
      <td>Gets the file name without the extension of the the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_full">path_get_full(path)</a></td>
      <td>Gets the absolute path for the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_invalid_file_name_chars">path_get_invalid_file_name_chars()</a></td>
      <td>Gets an array containing the invalid characters that are not allowed in file names.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_invalid_chars">path_get_invalid_chars()</a></td>
      <td>Gets an array containing the invalid characters that are not allowed in path names.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_root">path_get_root()</a></td>
      <td>Gets the root directory of the specified path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_random_file_name">path_get_random_file_name()</a></td>
      <td>Gets a random folder or file name.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_temp_file_name">path_get_temp_file_name()</a></td>
      <td>Creates a uniquely named temporary file on disk and returns the full path of that file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_get_temp">path_get_temp()</a></td>
      <td>Gets the path to the users temporary folder.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_has_extension">path_has_extension(path)</a></td>
      <td>Determines whether a path includes a file name extension.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/path_is_rooted">path_is_rooted(path)</a></td>
      <td>Determines if the specified path contains a root.</td>
    </tr>
  </tbody>
</table>
