GET /api/Table/tables
Response: Status Code 200

    {
      "data": [
        {
          "id": "4c63b2c6-3d38-413d-9b01-c0f94eddbe05",
          "tableNumber": "C6",
          "capacity": 4,
          "status": "\r\navailable",
          "description": "Ban Caffee",
          "createdAt": "2025-01-04T14:26:38",
          "updatedAt": "2025-01-04T14:26:38",
          "tableType": "Coffee"
        },
        {
          "id": "a157a1b8-f3fe-4f8c-810d-78b24afa7ae6",
          "tableNumber": "C5",
          "capacity": 5,
          "status": "available",
          "description": "Ban Caffee",
          "createdAt": "2025-01-04T14:23:47",
          "updatedAt": "2025-01-04T14:23:47",
          "tableType": "Coffee"
        }
      ],
      "pagination": {
        "totalResults": 12,
        "totalPages": 6,
        "currentPage": 1,
        "pageSize": 2
      }
    }

    Response: Status Code 404

    {
      "message": "No tables found matching the criteria."
    }

GET /api/Table/{id} (/api/Table/4c63b2c6-3d38-413d-9b01-c0f94eddbe05)

    Response: Status Code 200
    {
      "id": "4c63b2c6-3d38-413d-9b01-c0f94eddbe05",
      "tableNumber": "C6",
      "capacity": 4,
      "status": "available",
      "description": "Ban Caffee",
      "createdAt": "2025-01-04T14:26:38",
      "updatedAt": "2025-01-04T14:26:38",
      "tableType": "Coffee"
    }

    Response: Status Code 400

    {
      "msg": "Table not found."
    }

POST /api/Table

    Requestbody
    {
      "tableNumber": "A001",
      "capacity": 8,
      "status": "available",
      "description": "None",
      "typeId": "4067454d-a798-4d30-9e83-b04f451a9063"
    }
    Response: Status Code 200
    {
      "success": true,
      "message": "Table created successfully!",
      "data": {
        "id": "8fd52839-8013-42da-900e-31cd5a6a5818",
        "tableNumber": "B002",
        "capacity": 5,
        "status": "available",
        "description": "This is table B",
        "createdAt": "2025-03-17T16:21:56",
        "updatedAt": "2025-03-17T16:21:56",
        "tableType": "Coffee"
      }
    }
    Response: Status Code 400

    {
      "success": false,
      "message": "Shop ID does not exist!",
      "data": null
    }

    {
      "success": false,
      "message": "Table Type ID not found!",
      "data": null
    }

PUT /api/Table/{id}

Requestbody
{
"tableNumber": "A001",
"capacity": 8,
"status": "available",
"description": "None",
"typeId": "4067454d-a798-4d30-9e83-b04f451a9063"
}
Response: Status Code 200
{
"success": true,
"message": "Table updated successfully!",
"data": {
"id": "4c63b2c6-3d38-413d-9b01-c0f94eddbe05",
"tableNumber": "A001",
"capacity": 8,
"status": "available",
"description": "None",
"createdAt": "2025-01-04T14:26:38",
"updatedAt": "2025-03-17T16:13:05",
"tableType": "Coffee"
}
}
Response: Status Code 400
{
"success": false,
"message": "Table not found!",
"data": null
}

      {
          "success": false,
          "message": "Table Type ID not found!",
          "data": null
      }

DELETE /api/Table/{id}
Response: Status Code 200
{
"success": true,
"message": "Table has active bills. Status updated to 'Deleted'.",
"data": null
}

    {
    "success": true,
    "message": "Table deleted succesfully.",
    "data": null
    }
