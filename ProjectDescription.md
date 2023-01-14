### Overall description
## Product perspective
NPC is a web service that helps teachers track student's progress on projects.
## Product features
The service will have the following major features:
- authorization and authentication of users using built-in DotNet authorization;
- (For teacher)the ability to create a room(project) of students working on same project;
- the ability to create tasks in rooms for separate students and track their progress on them;
- abiity to add files to the room that will be accessible by every student;
- Login/log-out functionality

## Assumptions and dependencies
The system will be developed by using DotNet platform. Other dependencies will also
include:
● JavaScript for frontend;
● MSSQL;
● Ajax;
● AWS.
### Functional Requirements
This section contains all the functional requirements of this project. They describe how the
system should behave, so those requirements are essential and must be implemented in full.
- Login/Sign-up Page
The user is able to log in using emai and password or sign up.
- Settings Page
The user is able to sign out from the app.
- Rooms page with list of all rooms
Teacher can create new room with type, Project Name, Year fields and can add students by their emails.
- Room page
User can view Board with tasks for room, attachments and channel where users can type messages(basically it is chat) and view other's messages.
- Settings page
Users can view/change username, email, password and also log out from account. 
### Identity management
Users can create an account using email and password.
Types of roles: Student, Teacher.
User that have not authenticated yet:
- Can sign in/sign up.

Students that have already authenticated:
- Can view rooms they are assigned to;
- Can message in room chat;
- Can upload files to room;
- Can change their account info;
- Can update status of their tasks;
- Can sign out;

Teachers that have already authenticated:
- Can create rooms;
- Can edit rooms;
- Can add tasks;
- Can update task statuses of any student; 
- Can delete room;
- Can sign out.
### UML Diagram
![image](https://user-images.githubusercontent.com/58877099/212495042-e38b64b3-a3a1-4d1c-8c13-0995ce3c3c33.png)
