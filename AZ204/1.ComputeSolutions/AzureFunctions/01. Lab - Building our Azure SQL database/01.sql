-- Simple table for course orders
CREATE TABLE dbo.CourseOrders
(
    OrderId       INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName  NVARCHAR(100) NOT NULL,
    CustomerEmail NVARCHAR(200) NOT NULL,
    CourseName    NVARCHAR(200) NOT NULL,
    Amount        DECIMAL(10,2) NOT NULL,
    OrderDateUtc  DATETIME2(0)  NOT NULL DEFAULT (SYSUTCDATETIME())
);

-- Sample records
INSERT INTO dbo.CourseOrders (CustomerName, CustomerEmail, CourseName, Amount)
VALUES
('Customer 001', 'customer001@cloudxeus.com', 'AZ-204: Azure Developer Associate', 49.00),
('Customer 002', 'customer002@cloudxeus.com', 'AZ-104: Azure Administrator',       39.00),
('Customer 003', 'customer003@cloudxeus.com', 'DP-600: Fabric Analytics Engineer', 59.00),
('Customer 004', 'customer004@cloudxeus.com', 'AZ-305: Azure Architect',           69.00),
('Customer 005', 'customer005@cloudxeus.com', 'AI-900: Azure AI Fundamentals',     19.00);

-- Quick check
SELECT TOP (10) *
FROM dbo.CourseOrders
ORDER BY OrderId DESC;
