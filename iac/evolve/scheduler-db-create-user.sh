#!/bin/sh

ORIGINAL_IFS="$IFS"

LIST_ITEM_SEPARATOR="${LIST_ITEM_SEPARATOR:-;}"
LOCATIONS_SEPARATOR="${LOCATIONS_SEPARATOR:-$LIST_ITEM_SEPARATOR}"
PLACEHOLDERS_SEPARATOR="${PLACEHOLDERS_SEPARATOR:-$LIST_ITEM_SEPARATOR}"
SCHEMAS_SEPARATOR="${SCHEMAS_SEPARATOR:-$LIST_ITEM_SEPARATOR}"

LOCATIONS_ARGS=""
IFS="$LOCATIONS_SEPARATOR"
for LOCATION in $SCHEDULER_DB_CREATE_USER_LOCATIONS; do
    LOCATIONS_ARGS="$LOCATIONS_ARGS -l $LOCATION"
done

PLACEHOLDERS_ARGS=""
if [ -n "$SCHEDULER_DB_CREATE_USER_PLACEHOLDERS" ]; then
    IFS="$PLACEHOLDERS_SEPARATOR"
    for PAIR in $SCHEDULER_DB_CREATE_USER_PLACEHOLDERS; do
        PLACEHOLDERS_ARGS="$PLACEHOLDERS_ARGS -p $PAIR"
    done
fi

SCHEMA_ARGS=""
if [ -n "$SCHEMA" ]; then
    IFS="$SCHEMAS_SEPARATOR"
    for SCHEMA_ITEM in $SCHEMA; do
        SCHEMA_ARGS="$SCHEMA_ARGS -s $SCHEMA_ITEM"
    done
fi

IFS="$ORIGINAL_IFS"

METADATA_TABLE_ARG=""
if [ -n "$METADATA_TABLE" ]; then
    METADATA_TABLE_ARG="--metadata-table $METADATA_TABLE"
fi

# shellcheck disable=SC2086
/app/evolve_${EVOLVE_VERSION}_Linux-64bit/evolve $COMMAND $DBMS -c "${SCHEDULER_DB_CREATE_USER_CONNECTION_STRING}" $LOCATIONS_ARGS $SCHEMA_ARGS $PLACEHOLDERS_ARGS $METADATA_TABLE_ARG
