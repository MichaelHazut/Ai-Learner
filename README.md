# Educational Content Assessment Platform

This platform allows users to submit educational materials such as PDF files and presentations. The system then analyzes these materials to generate tests with multiple-choice questions, enabling users to evaluate their understanding of the content.

## Introduction

Our Educational Content Assessment Platform is designed to help individuals and educators create interactive tests from their study materials. Users can upload their educational content, and our platform automatically creates a quiz based on the information within those materials. This innovative approach provides a dynamic way to assess knowledge and understanding, making it a valuable tool for both self-study and educational institutions.

## Technologies

- ASP.NET
- MSSQL
- Entity Framework
- Angular
- TailWind CSS

## Features

- **Material Submission:** Users can upload study materials in various formats, including PDFs and presentations.
- **Automatic Test Generation:** The platform analyzes the uploaded materials and creates multiple-choice questions related to the content.
- **Interactive Testing:** Users can take the generated tests to assess their knowledge or understanding of the submitted materials.
- **Feedback System:** After completing tests, users receive immediate feedback, helping them identify areas of strength and improvement.

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

Before you begin, ensure you have the following tools installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/en/download/)
- [Angular CLI](https://cli.angular.io/)

### Installing

To get a development environment running:

1. Clone the repository:
```git clone https://github.com/MichaelHazut/Ai-Learner```

2. Navigate to the project directory:
```cd Ai-Learner```

3. Install the required ASP.NET and Angular dependencies:
```
dotnet restore
npm install
```
4. Start the development server:
```
dotnet run
ng serve --ssl true
```

5. Access the application at `https://localhost:4200/`.
6. Access backend swagger at `https://localhost:7089/swagger/index.html`
