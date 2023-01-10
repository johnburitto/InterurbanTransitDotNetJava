package com.encryptionserver.encryptionserver;

import org.springframework.data.mongodb.repository.MongoRepository;
import org.springframework.data.mongodb.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface KeyRepository extends MongoRepository<KeyModel, Integer> {
    @Query(value = " {isUsed :  {$eq : true}}")
    List<KeyModel> queryGetUsedKeys();

    @Query(value = " {isUsed :  {$eq : false}}")
    List<KeyModel> queryGetNotUsedKeys();
}
