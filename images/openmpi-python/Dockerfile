FROM alpine:edge

MAINTAINER Davit Petrosyan <davit.petrosyan@imast.io>

# enable community repository
RUN sed -i '/community/s/^#//' /etc/apk/repositories
RUN apk --no-cache --update-cache --available upgrade

# add missing packages (openssh, openmpi, ...)
RUN apk add --update --no-cache sudo openssh
RUN apk add --update --no-cache openmpi openmpi-dev
RUN apk add --update --no-cache python3 py3-pip python3-dev
RUN apk add --update --no-cache gcc g++ musl-dev
    
RUN ssh-keygen -A

# user and group definition for communication
ENV USR=shoc

# user and group identifiers
ENV USRID=1000

# home and ssh root folders
ENV HOMEROOT=/home/${USR}
ENV SSHROOT=${HOMEROOT}/.ssh

# create a user/group for inter container communication and add to sudoers 
RUN addgroup -g ${USRID} ${USR} && adduser -u ${USRID} -G ${USR} -D -g ${USR} ${USR} 

RUN echo "${USR} ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers

# set root password
RUN echo 'root:${USR}' | chpasswd 2> /dev/null 
RUN echo 'shoc:${USR}' | chpasswd 2> /dev/null 

# assign ssh root folder for the user
RUN mkdir -p ${SSHROOT}/

# home folder ownership
RUN chown -R ${USR}:${USR} ${HOMEROOT}/

# add configuration of ssh
ADD ssh/config ${SSHROOT}/config

# add configuration of sshd
ADD sshd/sshd_config /etc/ssh/sshd_config
ADD sshd/ssh_config /etc/ssh/ssh_config

RUN ssh-keygen -t rsa -f ${SSHROOT}/id_rsa -C ""

# add keys to shoc user ssh folder
RUN cp ${SSHROOT}/id_rsa.pub ${SSHROOT}/authorized_keys

# fix ssh root permissions 
RUN chmod -R 700 ${SSHROOT}/ 

# fix permission on keys
RUN chmod -R 600 ${SSHROOT}/authorized_keys

# correct ssh root folder ownership
RUN chown -R ${USR}:${USR} ${SSHROOT}/

# remove openmpi folder and re-create
RUN rm -fr ${HOMEROOT}/.openmpi && mkdir -p ${HOMEROOT}/.openmpi

# add parameters configuration
ADD mca-params.conf ${HOMEROOT}/.openmpi/mca-params.conf

# ownership of group
RUN chown -R ${USR}:${USR} ${HOMEROOT}/.openmpi/

WORKDIR /tmp
COPY requirements.txt .

RUN pip install -r requirements.txt 

# expose ssh port
EXPOSE 22

# run ssh service
CMD ["/usr/sbin/sshd", "-D"]
