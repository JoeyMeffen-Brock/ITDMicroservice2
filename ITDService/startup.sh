#!/usr/bin/sh
echo "SSHD_ENABLED = '${SSHD_ENABLED}'"
if [ ! -z "${SSHD_ENABLED}" ] && [ "${SSHD_ENABLED}" = "1" ]; then
  echo "Starting SSH ..."
	service ssh start
fi

dotnet ITDService.dll
