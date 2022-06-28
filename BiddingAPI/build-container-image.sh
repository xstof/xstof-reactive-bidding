IMAGE_NAME=bidding-api
REG_NAME=ccacr.azurecr.io
IMAGE_VERSION=latest

usage() {                                 # Function: Print a help message.
  echo "Usage: $0 [ -i IMAGE_NAME ] [ -r REGISTRY_NAME ] [ -v REGISTRY_VERSION ]" 1>&2 
}

exit_abnormal() {                       # Function: Exit with an error message.
  usage
  exit 1
}

while getopts i:r:v: flag
do
    case "${flag}" in
        i) IMAGE_NAME=${OPTARG};;
        r) REG_NAME=${OPTARG};;
        v) IMAGE_VERSION=${OPTARG};;
        :) exit_abnormal;;  # exptected argument missing
        *) exit_abnormal;;  # unexpected argument
    esac
done

FULL_IMAGE_NAME=${REG_NAME}/${IMAGE_NAME}:${IMAGE_VERSION}

echo "Building image $FULL_IMAGE_NAME"

DOCKER_BUILDKIT=0 docker build -t $FULL_IMAGE_NAME .