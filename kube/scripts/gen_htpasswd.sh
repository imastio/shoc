#!/bin/bash

FILE=$1

if [ -z "$FILE" ]
then
      echo "Path is empty."
	  echo "Example: $0 /path_to_file"
	  exit
fi

if [ ! -f "$FILE" ]; then
    echo "$FILE does not exists."
	exit
fi

RESULT=""
while IFS=':' read -r username password
do
  NEW_RESULT=$(htpasswd -Bbn $username $password)
  RESULT="$NEW_RESULT\n$RESULT" 
done < "$FILE"

echo -e -n $RESULT | base64 -w 0

printf "\n"

