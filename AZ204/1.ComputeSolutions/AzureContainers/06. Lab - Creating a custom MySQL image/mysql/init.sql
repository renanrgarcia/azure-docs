-- init.sql (MySQL)

-- Create database
CREATE DATABASE IF NOT EXISTS cloudxeusdb;
USE cloudxeusdb;

-- Create app user (lab-friendly)
CREATE USER IF NOT EXISTS 'appuser'@'%' IDENTIFIED BY 'StrongPassword!123';
GRANT ALL PRIVILEGES ON cloudxeusdb.* TO 'appuser'@'%';
FLUSH PRIVILEGES;

-- Create table
CREATE TABLE IF NOT EXISTS CourseOrders
(
    OrderId       INT AUTO_INCREMENT PRIMARY KEY,
    CustomerName  VARCHAR(100) NOT NULL,
    CustomerEmail VARCHAR(200) NOT NULL,
    CourseName    VARCHAR(200) NOT NULL,
    Amount        DECIMAL(10,2) NOT NULL,
    OrderDateUtc  DATETIME NOT NULL DEFAULT (UTC_TIMESTAMP())
);

-- Seed data
INSERT INTO CourseOrders (CustomerName, CustomerEmail, CourseName, Amount)
VALUES
('Customer 001', 'customer001@cloudxeus.com', 'AZ-204: Azure Developer Associate', 49.00),
('Customer 002', 'customer002@cloudxeus.com', 'AZ-104: Azure Administrator',       39.00),
('Customer 003', 'customer003@cloudxeus.com', 'DP-600: Fabric Analytics Engineer', 59.00),
('Customer 004', 'customer004@cloudxeus.com', 'AZ-305: Azure Architect',           69.00),
('Customer 005', 'customer005@cloudxeus.com', 'AI-900: Azure AI Fundamentals',     19.00);