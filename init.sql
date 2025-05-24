CREATE TABLE IF NOT EXISTS CarFactories (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Country VARCHAR(100)
);


CREATE TABLE IF NOT EXISTS Cars (
    Id SERIAL PRIMARY KEY,
    CarFactoryId INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Type VARCHAR(50),
    CONSTRAINT fk_carfactory
        FOREIGN KEY (CarFactoryId) 
        REFERENCES CarFactories(Id)
        ON DELETE CASCADE
);