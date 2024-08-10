CREATE TABLE "User" (
  "Id" uuid PRIMARY KEY,
  "FirstName" varchar NOT NULL,
  "LastName" varchar NOT NULL,
  "Location" varchar NOT NULL,
  "ProfileImageUrl" varchar NOT NULL,
  "RoleId" uuid,
  "BusinessId" uuid,
  "CreatedAt" timestamp NOT NULL,
  "UpdateAt" timestamp NOT NULL,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Role" (
  "Id" uuid PRIMARY KEY,
  "RoleName" varchar NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Website" (
  "Id" uuid PRIMARY KEY,
  "Name" varchar NOT NULL,
  "PhotoUrl" varchar NOT NULL,
  "URL" varchar NOT NULL,
  "CreatedAt" timestamp NOT NULL,
  "UpdateAt" timestamp NOT NULL,
  "IsAssigned" bool NOT NULL DEFAULT false,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Review" (
  "Id" uuid PRIMARY KEY,
  "UserId" uuid,
  "WebsiteId" uuid,
  "ReviewText" varchar NOT NULL,
  "ReviewScore" int NOT NULL,
  "CreatedAt" timestamp NOT NULL,
  "UpdateAt" timestamp NOT NULL,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Business" (
  "Id" uuid PRIMARY KEY,
  "WebsiteId" uuid,
  "IsConfirmed" bool NOT NULL DEFAULT false
);

ALTER TABLE "User" ADD FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id");

ALTER TABLE "User" ADD FOREIGN KEY ("BusinessId") REFERENCES "Business" ("Id");

ALTER TABLE "Review" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "Review" ADD FOREIGN KEY ("WebsiteId") REFERENCES "Website" ("Id");

ALTER TABLE "Business" ADD FOREIGN KEY ("WebsiteId") REFERENCES "Website" ("Id");
