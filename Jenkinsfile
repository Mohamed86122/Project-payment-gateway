pipeline {
    agent any

    environment {
        BUILD_CONFIGURATION = 'Release'          // Configuration du build (Release/Debug)
        DOTNET_PROJECT = 'AppTest/AppTest.csproj' // Chemin du fichier .csproj
        DOCKER_IMAGE = 'aspnet-core-webapi'      // Nom de l'image Docker
        DOCKER_TAG = 'latest'                    // Tag de l'image Docker
    }

    stages {
        stage('Checkout Code') {
            steps {
                script {
                    echo 'Cloning repository...'
                    checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [], userRemoteConfigs: [[url: 'https://github.com/Mohamed86122/Project-payment-gateway']]])
                }
            }
        }

        stage('Restore Dependencies') {
            steps {
                script {
                    echo 'Restoring dependencies...'
                    sh "dotnet restore \"${env.DOTNET_PROJECT}\""
                }
            }
        }

        stage('Build Project') {
            steps {
                script {
                    echo 'Building the project...'
                    sh "dotnet build \"${env.DOTNET_PROJECT}\" -c ${env.BUILD_CONFIGURATION}"
                }
            }
        }

        stage('Run Tests') {
            steps {
                script {
                    echo 'Running tests...'
                    sh "dotnet test \"${env.DOTNET_PROJECT}\" -c ${env.BUILD_CONFIGURATION}"
                }
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    echo 'Building Docker image...'
                    sh "docker build -t ${env.DOCKER_IMAGE}:${env.DOCKER_TAG} ."
                }
            }
        }

        stage('Push Docker Image to DockerHub') {
            steps {
                script {
                    echo 'Pushing Docker image to DockerHub...'
                    withCredentials([usernamePassword(credentialsId: 'dockerhub_credentials', passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                        sh '''
                        echo $DOCKER_PASSWORD | docker login -u $DOCKER_USERNAME --password-stdin
                        docker tag ${DOCKER_IMAGE}:${DOCKER_TAG} $DOCKER_USERNAME/${DOCKER_IMAGE}:${DOCKER_TAG}
                        docker push $DOCKER_USERNAME/${DOCKER_IMAGE}:${DOCKER_TAG}
                        '''
                    }
                }
            }
        }
    }

    post {
        success {
            script {
                echo 'Pipeline completed successfully!'
            }
        }
        failure {
            script {
                echo 'Pipeline failed. Please check the logs.'
            }
        }
    }
}
