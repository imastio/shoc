FROM alpine:edge

MAINTAINER Davit Petrosyan <davit.petrosyan@imast.io>

#enable community repository
RUN sed -i '/community/s/^#//' /etc/apk/repositories
RUN apk --no-cache --update-cache --available upgrade

# add missing packages (openssh, openmpi, ...)
RUN apk add --update --no-cache sudo openssh
RUN apk add --update --no-cache openmpi 

# user and group definition for communication
ENV USR=shoc
ENV GRP=shoc

# user and group identifiers
ENV USRID=1000
ENV GRPID=1000

# home and ssh root folders
ENV HOME=/home/${USR}
ENV SSHROOT=${HOME}/.ssh/

# create a user/group for inter container communication and add to sudoers 
RUN addgroup -S ${GRP} -g ${GRPID} --disabled-password --gecos "" && adduser -u ${USRID} -S ${USR} -G ${GRP}
RUN echo "${USR} ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers

# home folder ownership
RUN chown -R ${USR}:${GRP} ${HOME}/

# assign ssh root folder for the user
RUN mkdir -p ${SSHROOT}

# set root password
RUN echo 'root:${USR}' | chpasswd

# passwordless login
RUN sed -i 's/PermitRootLogin without-password/PermitRootLogin yes/' /etc/ssh/sshd_config

# maybe not needed anymore. SSH login fix. Otherwise user is kicked off after login
RUN sed 's@session\s*required\s*pam_loginuid.so@session optional pam_loginuid.so@g' -i /etc/pam.d/sshd

# add configuration of ssh
ADD ssh_default_config ${SSHDIR}/config


# fix ssh root permissions 
RUN chmod -R 600 ${SSHROOT}* 

# correct ssh root folder ownership
RUN chown -R ${USR}:${GRP} ${SSHROOT}

# do as root
USER root

# remove openmpi folder and re-create
RUN rm -fr ${HOME}/.openmpi && mkdir -p ${HOME}/.openmpi

# add parameters configuration
ADD mca-params.conf ${HOME}/.openmpi/mca-params.conf

# ownership of group
RUN chown -R ${USR}:${GRP} ${HOME}/.openmpi/

# expose ssh port
EXPOSE 22

CMD ["/usr/sbin/sshd", "-D"]
