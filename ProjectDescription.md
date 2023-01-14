### Overall description
## Product perspective
NPC is a web service that helps teachers track student's progress on projects.
## Product features
The service will have the following major features:
- authorization and authentication of users using built-in DotNet authorization;
- (For teacher)the ability to create a room of students working on same project;
- the ability to create tasks in rooms;
- abiity to add files to the room that will be accessible by every student;
- Login/log-out functionality

## Assumptions and dependencies
The system will be developed by using DotNet platform. Other dependencies will also
include:
● JavaScript;
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
- Can sign out;

Teachers that have already authenticated:
- Can create rooms;
- Can edit rooms;
- Can add tasks;
- Can add attachments to room;
- Can update task statuses of any student; 
- Can delete room;
- Can sign out.
### UML Diagram
![image](https://user-images.githubusercontent.com/58877099/212495042-e38b64b3-a3a1-4d1c-8c13-0995ce3c3c33.png)


### Architecture
For this project we used cloud based architecture. 
Basically porject is split in 3 parts: Frontend, Backend API and DB. 
In frontend we use JS to call API and API uses MVC architecture framework which is common for DotNet. 
The Model-View-Controller (MVC) is an architectural pattern that separates an application into three main logical components: the model, the view, and the controller. Each of these components are built to handle specific development aspects of an application.
The Model component corresponds to all the data-related logic that the user works with. The View component is used for returning aggregated data to frontend. Controllers act as an interface between Model and View components to process all the business logic and incoming requests, manipulate data using the Model component and interact with the Views to render the final output.

AWS infrastructure looks:
![image](https://user-images.githubusercontent.com/58877099/212496072-fa7bd970-4332-4d2a-80bb-38eb484b1c26.png)

Request get to API Gateway then it is passed to EC2 frontend which triggers backend API instance. Backend is connected to db. Uploaded files are stored on EC2 backend instance and path to them in DB.
Front end and backend are split because then there can be set some limit of requests between them and that makes system more configurable. Also if we want to create a mobile app then we will only need to create one frontend instance because backend is created and returns only data.
Frontend and backend are communicationg with HTTP and requests to API Gateway are sent with HTTPS it is handled out-of-box. Also this improves project security and makes it harder to forge access tokens.
Also DB is split into another service to improve project sustainability. If we need to run any migration/change in DB we can just duplicate it and replace connection settings when updates are done. 


### Concurrency pattern
We use DotNet which has built-in concurrency for request handling. Basicaly for each request it creates a new thread and after it is proccessed the thread gets emptied and memory freed. So amount of concurrent requests is limited only by performance of EC2 machine. Also all request handlers are done in asynchronus matter so for each request to db we can do another request while awaiting for response for previous one. That is done everywhere where it makes sense and can be done. 
Also for future we can scale up the project by adding several more EC2 instances and changing LoadBalancer configs. Then we can even configure automatic EC2 start when all instances are overwhelmed. 
