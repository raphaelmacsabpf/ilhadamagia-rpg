
exports.up = function(knex) {
    return knex.raw(`
        ALTER TABLE imtb_account 
        CHANGE COLUMN Money Money BIGINT NOT NULL DEFAULT '0' ,
        CHANGE COLUMN Bank Bank BIGINT NOT NULL DEFAULT '0' ;`
    );
};

exports.down = function(knex) {
    return knex.raw(`
        ALTER TABLE imtb_account 
        CHANGE COLUMN Money Money INT NOT NULL DEFAULT '0' ,
        CHANGE COLUMN Bank Bank INT NOT NULL DEFAULT '0' ;`
    );
};
