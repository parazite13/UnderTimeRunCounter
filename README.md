# UnderTimeRunCounter
Component of livesplit that display the number of runs that reach the end under a predefined time

## Install

 - Get the latest LiveSplit.UnderTimeRunCounter.dll from the release.
 - Copy the dll into the folder LIVESPLIT_INSTALL_FOLDER\Components\
 - Run LiveSplit
 - Right Click > Edit Layout... > "+" > Information > Under Time Run Counter

## Settings

In the layout settings you can set the target time under which a run will be counted. 
You can also assign the label that will be displayed in the Layout. For example it can be "Sub xx:xx Counter".

## Build

 - Clone the main LiveSplit repository from : https://github.com/LiveSplit/LiveSplit
 - Add this repository as a submodule : git submodule add "https://github.com/parazite13/UnderTimeRunCounter.git" "LiveSplit/Components/LiveSplit.UnderTimeRunCounter"
 - Add the LiveSplit.UnderTimeRunCounter.csproj in the solution as a new project under the Components > Information folder