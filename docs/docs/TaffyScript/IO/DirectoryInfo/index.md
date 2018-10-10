---
layout: default
title: DirectoryInfo
---

# DirectoryInfo

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/)

_Exposes scripts for creating, moving, and iterating through directories._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/attributes/">attributes</a></td>
      <td>[FileAttributes](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileattributes)</td>
      <td>Gets or sets the attributes for the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/creation_time/">creation_time</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the creation time of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/creation_time_utc/">creation_time_utc</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the creation time in coordinated universal time of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/exists/">exists</a></td>
      <td>bool</td>
      <td>Determines if the directory exists.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/extension/">extension</a></td>
      <td>string</td>
      <td>Gets the extension part of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/full_name/">full_name</a></td>
      <td>string</td>
      <td>Gets the full path of the directory</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/last_access_time/">last_access_time</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was accessed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/last_access_time_utc/">last_access_time_utc</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was accessed in coordinated universal time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/last_write_time/">last_write_time</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was written to.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/last_write_time_utc/">last_write_time_utc</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>Gets or sets the last time the directory was written to in coordinated universal time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/name/">name</a></td>
      <td>string</td>
      <td>Gets the name of the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/parent/">parent</a></td>
      <td>[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)</td>
      <td>Gets the parent directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/root/">root</a></td>
      <td>[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)</td>
      <td>Gets the root portion of the directory</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/create">create()</a></td>
      <td>Creates the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/create_subdirectory">create_subdirectory(path)</a></td>
      <td>Creates a subdirectory with the specified path. The path can be relative to this directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/delete">delete([delete_subdirectories=false])</a></td>
      <td>Deletes this directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/enumerate_directories">enumerate_directories([search_pattern], [search_option=SearchOption.TopDirectoryOnly])</a></td>
      <td>Returns a collection of directory information in the current directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/enumerate_file">enumerate_file([search_pattern], [search_option=SearchOption.TopDirectoryOnly])</a></td>
      <td>Returns a collection of file information in the current directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/get_directories">get_directories([search_pattern], [search_option=SearchOption.TopDirectoryOnly])</a></td>
      <td>Returns an array of the subdirectories in the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/get_files">get_files([search_pattern], [search_option=SearchOption.TopDirectoryOnly])</a></td>
      <td>Gets an array of the files in the directory.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/move_to">move_to(destinationPath)</a></td>
      <td>Moves this directory and its contents to a new path.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/refresh">refresh()</a></td>
      <td>Refreshes the state of the directory.</td>
    </tr>
  </tbody>
</table>
