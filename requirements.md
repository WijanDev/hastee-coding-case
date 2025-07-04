# Hastee Coding Case: CSV Parser

## 📌 Interview Context

This interview focuses on your **coding skills**. You'll receive a task beforehand, and during our discussion, we'll review your proposed solutions. We might also explore your approach to understand how you incorporate feedback and diverse perspectives.

---

## 🧪 Task

Write a **small program skeleton (no need for full functionality)** in **C#** that can parse different CSV files and build a **common Data Transfer Object (DTO)**.

This exercise aims to evaluate your ability to:

- ✅ Design flexible and extensible solutions  
- ✅ Write clean, well-structured code  
- ✅ Understand requirements and manage uncertainty during system design

---

## 📄 Problem Description

You are tasked with creating a program that can parse CSV files with **varying structures**. The program should:

### 1. Accept Inputs:
- A CSV file path  
- A `ParserType` (string) to specify which parsing logic to use

### 2. Dynamically Parse:
- Parse the CSV file based on the provided `ParserType`
- Construct a **common DTO** to hold the parsed data

### 3. Handle Different Parser Types:
Different `ParserType` values may require different methods for:
- Validating data
- Transforming data
- Aggregating data from external resources (e.g., a database or API)

### 4. Reuse Methods:
- Design should allow methods to be **reused** across parser implementations

### 5. Handle Data Variations:
- Methods should support **single or multi-cell** data operations

### 6. Error Handling:
- Record **detailed validation or format errors** for reporting

---

## 📊 Example Scenario

Imagine you have two CSV files with different structures:

### Type A:
```
CustomerID, Full Name, Email, Phone, Salary
```

### Type B:
```
ID, Name, Surname, CorporateEmail, PersonalEmail, Salary
```

You’d need to implement different `ParserType`s (e.g., `TypeAParser`, `TypeBParser`) to:

- Transform them into the **common DTO**
- Apply **specific validation rules**
  - e.g., TypeA has a `FullName` that must match `<name> <surname>`
  - TypeB has `Name` and `Surname` separately
- Apply **specific extraction logic**
  - e.g., for email priority or fallback
- Possibly **load external data**
  - e.g., TypeB lacks `Phone`, to be retrieved via API using `ID`

---

## ✅ Requirements

### DTO Definition:
- Define a **common DTO class**
- Keep it simple (dictionary or generic properties acceptable)

### Parser Types:
- Implement a mechanism to **define and select** different parser types
- Suggested: Inheritance, Strategy Pattern, etc.
- Only a **structure is needed**, not full implementations

### Parsing Logic:
- Outline logic to parse the CSV file and **map data to the DTO**
- Demonstrate adaptability to different `ParserType`s

### Validation & Transformation:
- Show how these methods would be **defined, implemented and reused**
- Provide **examples** of such methods

### Error Handling:
- Implement a way to record and report **detailed parsing errors**
  - e.g., row number, expected vs. received, exceptions, etc.

---

## ⚠️ Constraints

- Solution must be in **C#**
- You **don't need to implement** full CSV parsing logic
- You can **assume external components/libraries** exist (file loading, API calls, etc.)
- Focus on **design and structure**, not full functionality
- Skeleton code is sufficient
- If using ad-hoc libraries or components, a **brief description is enough**

---

## 📤 Submission

Submit a **C# code file** containing the **program skeleton**.  
The code should be **well-commented** to explain design choices and implementation approach.
