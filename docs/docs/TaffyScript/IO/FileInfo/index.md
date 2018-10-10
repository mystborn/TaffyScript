---
layout: default
title: FileInfo
---

# FileInfo

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/)

_Provides mechanisms for the creation, copying, deletion, moving, and opening of files._

## Properties

<table>
  <col width="15%">
  <col width="15%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/attributes/">attributes</a></td>
      <td>[FileAttributes](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileattributes)</td>
      <td>Gets or sets the attributes for the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/creation_time/">creation_time</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the creation time of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/creation_time_utc/">creation_time_utc</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the creation time in coordinated universal time of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/exists/">exists</a></td>
      <td>bool</td>
      <td>Determines if the directory exists.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/extension/">extension</a></td>
      <td>string</td>
      <td>Gets the extension part of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/full_name/">full_name</a></td>
      <td>string</td>
      <td>Gets the full path of the directory</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/last_access_time/">last_access_time</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was accessed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/last_access_time_utc/">last_access_time_utc</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was accessed in coordinated universal time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/last_write_time/">last_write_time</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was written to.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/last_write_time_utc/">last_write_time_utc</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was written to in coordinated universal time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/name/">name</a></td>
      <td>string</td>
      <td>Gets the name of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/directory/">directory</a></td>
      <td>[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)</td>
      <td>Gets an instance of the parent directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/directory_name/">directory_name</a></td>
      <td>string</td>
      <td>Gets the name of the parent directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/is_read_only/">is_read_only</a></td>
      <td>bool</td>
      <td>Gets or sets a value that determines if the file read only.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/length/">length</a></td>
      <td>number</td>
      <td>Gets the size, in bytes, of the current file.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/append_text">append_text()</a></td>
      <td>Creates a StreamWriter that appends text to this file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/copy_to">copy_to(new_file_name, [overwrite=false])</a></td>
      <td>Copies the file to a new file, optionally allowing the overwrite of an existing file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/create">create()</a></td>
      <td>Creates the file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/create_text">create_text()</a></td>
      <td>Creates a StreamWriter that writes to a new text file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/decrypt">decrypt()</a></td>
      <td>Decrypts a file that was encrypted by the current account using encrypt.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/delete">delete()</a></td>
      <td>Permanently deletes this file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/encrypt">encrypt()</a></td>
      <td>Encrypts a file so only the account used to encrypt the file can decrypt it.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/move_to">move_to(new_file_name)</a></td>
      <td>Moves the file to a new location.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/open">open(mode, [access], [share])</a></td>
      <td>Opens the file with the specified options.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/open_read">open_read()</a></td>
      <td>Creates a read-only FileStream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/open_text">open_text()</a></td>
      <td>Creates a StreamReader that reads from an existing text file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/open_write">open_write()</a></td>
      <td>Creates a write-only FileStream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/refresh">refresh()</a></td>
      <td>Refreshes the state of this file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileInfo/replace">replace(destination_file_name, destination_backup_file_name, [ignore_metadata_errors])</a></td>
      <td>Replaces the contents of the specified file with the contents of this file.</td>
    </tr>
  </tbody>
</table>
