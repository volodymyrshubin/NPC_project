# Entity Relationship Diagram
![Untitled](https://user-images.githubusercontent.com/58877099/212547557-200f53bc-deb9-4ae7-b83d-2a52d76bb0c6.jpg)

## Details:

### Users:
- Id : user id, Primary Key 
- role: User role: Teacher or Student
- email: user email
- Name: student or teacher name
- Surname student or teacher surname
- PasswordHash: varchar, hash of password

### Rooms:
Contains list of all student rooms.
- Id: room id, Primary Key 
- ProjectName: Name of the project
- Year: contains year of studiyng
- ProjectType: specifies type of project
- isCompleted: stage of project, only teacher can change it
- CreatedOn: when room was created
- CreatedBy: Foreign Key on users.Id, teacher who created the room 

### RoomFiles
Contains list of files that are in the room
- Id: file id, PrimaryKey
- RoomId: Foreign Key on rooms.Id, specifies in which room files were uploaded
- StudentId: Id of student who uploaded file, Foreign Key on User.Id
- FileName: name of the file.
- FilePth: path to file

### StudentRooms
Table to combine users and rooms
- Id: table row id, Primary Key
- SudentId: Foreing Key on users.Id
- RoomId: Foreing Key on Rooms.Id

### ProjectTasks
Table containing tasks for students in rooms
- Id: Primary Key
- RoomId: Foreign Key on Rooms.Id. Where the task was created in which room
- StudentId: Foreign Key on users, who this task was assigned to 
- Name: name of the task
- Description: description of the task
- Status: status of the task, students can modify status by themselves 


## Purpose and types of data
We collect and store files for students and teachers to access them. Every file can be deleted by user who uploaded it. We do not store deleted files, meaning that if it is deleted that it is deleted from our service

We store data about users for a period of 1 year of inactivity. Also users can ask to delete their accounts and all data linked to that account.

## Retention period
We retain user data as long as user is willing to. We store rooms data for a period of 1 year after they are expired (baser on Year column). 
Files are stored untill they are deleted by user who uploaded them or untill user wants to delete his account. 

## Access to data and security measures
Access to files and user data is granted only to authorized employees and contractors who require access for the purpose of maintaining the service or responding to user requests.

We implemented security measures to protect files and data from unauthorized access, use, or disclosure, including encryption and secure storage. Users can request the deletion of their data at any time by contacting our support.

# Resiliency model
![image](https://user-images.githubusercontent.com/58877099/212549918-9fd98165-e843-4c5c-9583-82bddfff754b.png)

| Name  | CDI  |  Integration Description |
|---|---|---|
| API Gateway  | API Gateway -> Load Balancer  | Make GET/POST/PUT/PATCH requests to service, also provides more managment on request quantity  |
| Elastic Load Balancer |Load Balancer -> EC2 instance   | Make sure servers can accept new requests, if not then automatically setup new instance. Pass request to EC2  |
| EC2 instance  | EC2 -> DB  | CRUD operations with DB  |

# Application security Model

Application security models have several attributes that need to be addressed at each layer of the application. Our application satisfies the following security principles:

### User Management:
The application provides the ability to create, delete, and modificate users, rooms, and files within an identity system support of application. The modifications can include adding or removing attributes(managing permissions), changing passwords and similar activities.

### Authentication:
Authentication is the most generic concept that provides your identity to the system and it is a common way to handle security for all applications. There is one way through which the user is able to prove his identity to the system:

Cookie based authentication The client posts the login credential to the server, server verifies the credential and creates session id which is stored in server(state-full) and returned to client via set-cookie. On subsequent request the session id from the cookie is verified in the server and the request gets processed. Upon logout session id will be cleared from both client cookie and server.

### Authorization:
The process of determining if the authenticated identity is allowed to access the requested server resource based upon a predefined authorization policy.

### Confidentiality:
Only users who you shared access can view the files or the data you shared.

### Identity Propagation:
A mechanism that, ideally, securely transmits an authenticated identity from one system actor to another (JWT tokens).

The application is resistant to SQL injections and Cross-site scripting when attacker tries to gain access to private information by delivering malicious code to end-users via trusted Web site.

# Infrastructure Diagram 

![image](https://user-images.githubusercontent.com/58877099/212549918-9fd98165-e843-4c5c-9583-82bddfff754b.png)

Webservice consists of two parts: frontend and backend. The user will interact with the client part, which interacts with the server part to provide and process all the necessary data.

Client is implement in multipage way so it requires reloading page during use in browser.

The client sends HTTP requests to get the required data from the backend. This is done using AJAX technology. The server has an implemented HTTP-Server that waits for incoming requests. During authentication the server part generates cookies for the user with an identifier that is encrypted. The browser stores this data.

We use API Gateway as a proxy between users and servers. That is done to have more configuration options and to have option to limit request to server.
We use Elastic Load Balancer to balance server load and also it can roll-up new EC2 instances and interact with them.
We use EC2 for hosting Backend and Frontend for our application.

# Analytics model

| Metric                                                | Unit         |                              |                                                       |
|-------------------------------------------------------|--------------|------------------------------|-------------------------------------------------------|
| Average user per month                                | Number       |                              | [Funnel 1](red)                                              |
| Average size of uploaded file by user                 | MegaBytes    |                              | Average size of uploaded file by user                 |
| Number of accounts deleted monthly                    | Number       |                              | Average quantity of files uploaded by user            |
| Number of accounts deleted yearly                     | Number       |                              | Average server response time                          |
| Average user sign-up per month                        | Number       |                              |                                                       |
| Average quantity of files uploaded by user            | Number       |                              | [Funnel 2](red)                                              |
| Sign up to creating a room conversion                 | Percentage   |                              | Sign up to creating a room conversion                 |
| Creating a room to first file upload conversion       | Percentage   |                              | Creating a room to first file upload conversion       |
| First file upload to first message in chat conversion | Percentage   |                              | First file upload to first message in chat conversion |
| Average time on service                               | Hours        |                              |                                                       |
| Average room size                                     | Number       |                              |                                                       |
| Average server response time                          | Milliseconds |                              |                                                       |
| Average requests to server per hour                   | Number       |                              |                                                       |
| Number of active users daily                          | Number       |                              |                                                       |
| Number of active users monthly                        | Number       |                              |                                                       |

Key functional metrics to track project performance

# Monitoring and Alerting model

| Metric                              | Unit         | Min | Max  | Priority |
|-------------------------------------|--------------|-----|------|----------|
| Unit test coverage                  | Percentage   | 85% |      | Medium   |
| Uptime                              | Percentage   | 95% |      | High     |
| Average reponse time                | Miliseconds  |     | 1000 | Medium   |
| Server load                         | Percentage   |     | 80   | High     |
| Probability of Failure on demand    | Percentage   |     | 2    | Low      |
| Failed requests quantity            | Percentage   |     | 5    | High     |
| Failed i/o operations in db         | Percentage   |     | 10   | High     |
| Requests lost between ELB and EC2   | Percentage   |     | 2    | High     |
| Failures of retrieving uploads      | Percentage   |     | 5    | Medium   |
| Database SELECT operation max time  | Miliseconds  |     | 500  | Medium   |

Key metrics to track performance of service


