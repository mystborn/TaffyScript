---
layout: default
title: Grid
---

# Grid

[\[global\]]({{site.baseurl}}/docs/).[Grid]({{site.baseurl}}/docs/Grid/)

_Represents a grid of objects that can be accessed by an x and y coordinate._

## Fields

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
      <td><a href="{{page.url}}width/">width</a></td>
      <td>number</td>
      <td>Gets the width of the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}height/">height</a></td>
      <td>number</td>
      <td>Gets the height of the grid</td>
    </tr>
  </tbody>
</table>

## Constructor

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
      <td><a href="{{page.url}}create/">create(width, height)</a></td>
      <td>Creates a new grid with the specified dimensions.</td>
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
      <td><a href="{{page.url}}add">add(x, y, value)</a></td>
      <td>Adds a given value to the value at the specified position.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}add_disk">add_disk(x_middle, y_middle, radius, value)</a></td>
      <td>Adds a given value to all values inside of a disk.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}clear">clear(default_value)</a></td>
      <td>Sets all cells in the grid to the specified value.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}copy">copy()</a></td>
      <td>Creates a shallow copy of the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}get">get(x, y)</a></td>
      <td>Gets the value at the specified position within the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}resize">resize(width, height)</a></td>
      <td>Resizes the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}set">set(x, y, value)</a></td>
      <td>Sets a position within the grid to a specified value.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}shuffle">shuffle()</a></td>
      <td>Shuffles all of the cells in the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}sort">sort(column, ascending)</a></td>
      <td>Sorts a column of the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}value_exists_in_disk">value_exists_in_disk(x_middle, y_middle, radius, value)</a></td>
      <td>Determines if a value exists within a a disk in the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}value_position_in_disk">value_position_in_disk(x_middle, y_middle, radius, value)</a></td>
      <td>Gets the x and y values of a value within a disk on the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}value_exists_in_region">value_exists_in_region(x1, y1, x2, y2, value)</a></td>
      <td>Determines if a value exists within a region on the grid.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}value_position_in_region">value_position_in_region(x1, y1, x2, y2, value)</a></td>
      <td>Gets the x and y values of a value within a region on the grid.</td>
    </tr>
  </tbody>
</table>
