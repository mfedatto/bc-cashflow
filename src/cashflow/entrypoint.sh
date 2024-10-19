#!/bin/bash

if [ "$BC_APP" == "web" ]; then
    exec dotnet web/Bc.CashFlow.Web.dll --urls "http://*: ${WEB_PORT}"
elif [ "$BC_APP" == "scheduler" ]; then
    exec dotnet scheduler/Bc.CashFlow.Scheduler.dll --urls "http://*: ${SCHEDULER_PORT}"
else
    echo "Error: environment variable BC_APP must be 'web' or 'scheduler'."
    exit 1
fi
