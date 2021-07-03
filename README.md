## Introduction
The Assistive Context Aware Toolkit (ACAT) supports a variety of sensors including ACAT Vision (camera based sensor) and most commercially available "off-the-shelf" sensors. However, for some users, using the camera based sensor may not be feasible due to their condition and the commercially available sensors may be too expensive. To bridge this gap, Intel Labs has developed an Arduino based proximity sensor solution that is also open source. If the potential ACAT user has financial constraints that prevent them from purchasing "off-the-shelf" sensors and has someone with minimal technical experience to help set up the Arduino proximity sensor, then this system may be the best solution for them. Additionally, the system is open source so developers can build on top of the system by adding new sensors, features, algorithms, etc.

The VCNL4010 proximity sensor used in the system best operates by detecting movements close to the sensor (within ~4cm). Some examples of ideal movements that could be detected by the sensor include: hand / finger movements, facial movements, and leg / foot movements. Set up of the sensor requires some soldering (about a couple minutes), finding someone that already knows how to do so is recommended. The sensor has holes for screws so it is flexible in terms of mounting options. Please see below video for a demo of the system.

https://youtu.be/DFuzGPFUG50


## Project Website
Click [here](http://01.org/acat) for the ACAT website where more information on the Arduino Proximity Sensor can be found


## Arduino Code Overview
The Arduino code is in C++ and any text editor or IDE compatible with the C++ programming language can be used to modify that code.

ACAT_Arduino_Proximity_Sensor.ino – Main Arduino "sketch file". Loaded into Arduino Software IDE and connects the proximity sensor logic to the AcatActuator logic
VCNL4010 – VCNL4010 proximity sensor driver
Actuator – Processes the proximity data and sends messages to and receives messages from the Windows interface app
cppQueue – Queue library used in the peak detection algorithm data processing


## Windows Interface App (AcatActuator) Code Overview
The Windows Interface App (AcatActuator) was implemented in C# using Microsoft Visual Studio Professional 2017 with Microsoft .NET framework 4.7

Program.cs – Main launching point of the application
AcatActuator.cs – Initializes forms and serial connection to Arduino
Form1.cs – Code for main window
FormFeedback.cs – Code for feedback window
SerialComm.cs – Establishes serial connection to Arduino and sends ACAT trigger events
Chart.cs – Chart component in main form
SensorPositionTracker.cs – Tracks the sensor positon when there is no movement and gives recommendations on sensor placement
WindowOverlapWatchdog.cs – Makes sure feedback window goes on top of all other windows except ACAT ones
Settings.cs – Keeps track of Arduino and application settings loaded from App_Settings.xml


## Licensing
This project is distributed under the Apache License, Version 2.0.

Originally found on Dropbox, copied to GitHub for the ease of access and version control.

