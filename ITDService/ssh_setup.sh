#!/usr/bin/env bash
ssh-keygen -A

#prepare run dir
if [ ! -d "/var/run/sshd" ]; then
    mkdir -p /var/run/sshd
fi