# DOCKER_ENV=''
DOCKER_TAG=''
# DOCKER_ENV=docker

case "$TRAVIS_BRANCH" in
  "master")
    # DOCKER_ENV=production
    DOCKER_TAG=latest
    ;;
  "develop")
    # DOCKER_ENV=development
    DOCKER_TAG=dev
    ;;    
esac

echo "$DOCKER_PASSWORD" | docker login --username $DOCKER_USERNAME --password-stdin
docker build -t stock-advisor:$DOCKER_TAG ./src/StockAdvisor.Api --no-cache 
docker tag stock-advisor:$DOCKER_TAG $DOCKER_USERNAME/stock-advisor:$DOCKER_TAG
docker push $DOCKER_USERNAME/stock-advisor:$DOCKER_TAG